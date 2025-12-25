using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using DaccApi.Model;
using DaccApi.Helpers;
using DaccApi.Infrastructure.Cryptography;
using DaccApi.Infrastructure.Mail;
using DaccApi.Infrastructure.Repositories.Permission;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model.Responses;
using DaccApi.Responses;
using DaccApi.Services.Token;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IArgon2Utility _argon2Utility;
        private readonly ITokenService _tokenService;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMailService _mailService;
        private readonly string[] _userStartImages = [
            "https://i.postimg.cc/vHBtpYFp/invertidoshh.png",
            "https://i.postimg.cc/X7JkTjSR/anonimoshh.png",
            "https://i.postimg.cc/zXBF9zYY/linhashh.png"
        ];
        public AuthService(IUsuarioRepository u, IArgon2Utility a, ITokenService t, IPermissionRepository p, IMailService m)
        {
            _usuarioRepository = u;
            _argon2Utility = a;
            _tokenService = t;
            _permissionRepository = p;
            _mailService = m;
        }
        
        private bool ValidateCredentials(string email, string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            
            return _argon2Utility.VerifyPassword(password, hashedPassword);
        }
        
        public async Task<IActionResult> LoginUser(RequestLogin request)
        {
            try{
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
                {
                    var validationDetails = new List<object>();
                    if (string.IsNullOrEmpty(request.Email))
                    {
                        validationDetails.Add(new { field = "email", message = "Email é obrigatório" });
                    }
                    if (string.IsNullOrEmpty(request.Senha))
                    {
                        validationDetails.Add(new { field = "senha", message = "Senha é obrigatória" });
                    }

                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST.WithDetails(validationDetails.ToArray()));
                }

                var usuario = await _usuarioRepository.GetUserByEmail(request.Email);

                if (usuario is not { Ativo: true } || !ValidateCredentials(request.Email, request.Senha, usuario.SenhaHash!))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INVALID_CREDENTIALS);
                }

                var permissions = await _permissionRepository.GetPermissionsForRoleAsync(usuario.Cargo);

                var accessToken = _tokenService.GenerateAccessToken(usuario, permissions);
                var refreshToken = _tokenService.GenerateRefreshToken(usuario);

                // TODO: Colocar o ExpiresIn como argumento para atualizar/criar o token do usuário
                var expiresIn = new DateTimeOffset(DateTime.UtcNow.AddMinutes(15)).ToUnixTimeSeconds();

                await _usuarioRepository.UpdateUserTokens(usuario.Id,
                    new TokensUsuario(){AccessToken = accessToken, RefreshToken = refreshToken, ExpiresIn = expiresIn});

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(
                    new ResponseLogin
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        ExpiresIn = expiresIn,
                        User = usuario.ToResponse()
                    }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message + ex.StackTrace);
            }
        }
            
        // TODO: Transferir email para o controller e refatorar a lógica do register para retornar o usuário ou uma exception.
        public async Task<IActionResult> RegisterUser(RequestRegistro requestCreate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(requestCreate.Nome) ||
                    string.IsNullOrWhiteSpace(requestCreate.Sobrenome) ||
                    string.IsNullOrWhiteSpace(requestCreate.Curso))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }

                if (!string.IsNullOrWhiteSpace(requestCreate.Ra) && !IsValidRa(requestCreate.Ra))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST,
                        "RA inválido! Seu RA deve conter 9 dígitos numéricos");
                }

                if (!IsValidEmail(requestCreate.Email))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Formato de email inválido!");
                }

                if (!IsValidPassword(requestCreate.Senha))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, 
                        "Senha muito fraca! A senha deve ter ao menos 8 caracteres, " +
                        "uma letra maiúscula, uma letra minúscula e um número!");
                }

                var random = new Random();
                var randomNumber = random.Next(0, _userStartImages.Length);
                var randomStartImageUrl = _userStartImages[randomNumber];

                var usuario = new Usuario
                {
                    Nome = requestCreate.Nome,
                    Sobrenome = requestCreate.Sobrenome,
                    Ra = requestCreate.Ra,
                    Curso = requestCreate.Curso,
                    Email = requestCreate.Email,
                    ImagemUrl = randomStartImageUrl,
                    Telefone = requestCreate.Telefone,
                    SenhaHash = _argon2Utility.HashPassword(requestCreate.Senha),
                    Ativo = true,
                    InscritoNoticia = requestCreate.InscritoNoticia ?? false,
                    Cargo = CargoUsuario.Aluno,
                    DataCriacao = DateTime.Now,
                    DataAtualizacao = DateTime.Now
                };

                var userId = await _usuarioRepository.CreateUser(usuario);

                usuario.Id = userId;

                // envia email de boas vindas
                // await _mailService.SendWelcomeEmailAsync(usuario);
                
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new { users = usuario.ToResponse() }));
            }
            catch (InvalidConstraintException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,
                    "Ocorreu um erro ao tentar cadastrar o usuário, favor relatar ao suporte pelo: contato.daccfei@gmail.com " + ex.Message);
            }
        }

        public async Task<IActionResult> RefreshUserToken(string refreshToken)
        {
            try
            {
                Guid userId;
                try
                {
                    userId = Guid.Parse(new JwtSecurityToken(refreshToken).Claims
                        .First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                }
                catch (ArgumentException)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.AUTH_TOKEN_INVALID);
                }
                catch (FormatException)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.AUTH_TOKEN_INVALID);
                }
                
                var user = await _usuarioRepository.GetByIdAsync(userId);
                
                var validRefreshToken = await _tokenService.ValidateRefreshToken(userId, refreshToken);
                if (!validRefreshToken || user == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.AUTH_TOKEN_INVALID);
                }
                
                var userPermissions = await _permissionRepository.GetPermissionsForRoleAsync(user.Cargo);
                
                var newAccessToken = _tokenService.GenerateAccessToken(user, userPermissions);
                var newRefreshToken = _tokenService.GenerateRefreshToken(user);

                // TODO: Colocar o ExpiresIn como argumento para atualizar/criar o token do usuário
                var expiresIn = new DateTimeOffset(DateTime.UtcNow.AddMinutes(15)).ToUnixTimeSeconds();

                var userTokens = new TokensUsuario
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresIn = expiresIn
                };

                await _usuarioRepository.UpdateUserTokens(user.Id, userTokens);
                
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { userTokens }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message + ex.StackTrace);;
            }
        }

        public async Task<IActionResult> Logout(Guid userId)
        {
            try
            {
                var tokensUsuario = new TokensUsuario(){ AccessToken = "", RefreshToken = "", ExpiresIn = 0};
                
                await _usuarioRepository.UpdateUserTokens(userId, tokensUsuario);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Usuário saiu com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> ForgotPassword(RequestForgotPassword request)
        {
            try
            {
                var user = await _usuarioRepository.GetUserByEmail(request.Email);
                if (user == null)
                {
                    // Por segurança, não informamos se o email existe ou não
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Se o e-mail existir, um link de recuperação será enviado.");
                }

                var token = _tokenService.GenerateResetToken();
                var resetToken = new UsuarioResetToken
                {
                    UsuarioId = user.Id,
                    Token = token,
                    DataExpiracao = DateTime.UtcNow.AddMinutes(30)
                };

                await _usuarioRepository.SaveResetTokenAsync(resetToken);
                await _mailService.SendResetPasswordEmailAsync(user, token);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "E-mail de recuperação enviado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> ValidateResetToken(string token)
        {
            try
            {
                var resetToken = await _usuarioRepository.GetResetTokenAsync(token);
                if (!_tokenService.IsResetTokenValid(resetToken!))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Token inválido ou expirado.");
                }

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Token válido.");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> ResetPassword(RequestResetPassword request)
        {
            try
            {
                var resetToken = await _usuarioRepository.GetResetTokenAsync(request.Token);
                if (!_tokenService.IsResetTokenValid(resetToken!))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Token inválido ou expirado.");
                }

                if (!IsValidPassword(request.NewPassword))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Senha não atende aos requisitos de segurança.");
                }

                var newPasswordHash = _argon2Utility.HashPassword(request.NewPassword);
                await _usuarioRepository.UpdatePasswordAsync(resetToken!.UsuarioId, newPasswordHash);
                await _usuarioRepository.InvalidateResetTokenAsync(resetToken.Id);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Senha redefinida com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> ChangePassword(Guid userId, RequestChangePassword request)
        {
            try
            {
                var user = await _usuarioRepository.GetByIdAsync(userId);
                if (user == null) return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);

                if (!_argon2Utility.VerifyPassword(request.CurrentPassword, user.SenhaHash))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INVALID_CREDENTIALS, "Senha atual incorreta.");
                }

                if (!IsValidPassword(request.NewPassword))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Nova senha não atende aos requisitos de segurança.");
                }

                var newPasswordHash = _argon2Utility.HashPassword(request.NewPassword);
                await _usuarioRepository.UpdatePasswordAsync(userId, newPasswordHash);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Senha alterada com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        private static bool IsValidRa(string? ra)
        {
            if (ra == null || string.IsNullOrWhiteSpace(ra))
            {
                return false;
            }

            return ra.Any(char.IsDigit) && ra.Length == 9;
        }
        
        private static bool IsValidPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            if (password.Length < 8)
            {
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            return true;
        }
        
        private static bool IsValidEmail(string? email)
        {
            if (email == null || string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                var mail = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
