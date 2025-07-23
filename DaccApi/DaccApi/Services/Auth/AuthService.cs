using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Authentication;
using System.Security.Claims;
using DaccApi.Model;
using DaccApi.Helpers;
using DaccApi.Infrastructure.Cryptography;
using DaccApi.Infrastructure.Repositories.Permission;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model.Responses;
using DaccApi.Services.Token;
using Helpers.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IArgon2Utility _argon2Utility;
        private readonly ITokenService _tokenService;
        private readonly IPermissionRepository _permissionRepository;
        public AuthService(IUsuarioRepository u, IArgon2Utility a, ITokenService t, IPermissionRepository p)
        {
            _usuarioRepository = u;
            _argon2Utility = a;
            _tokenService = t;
            _permissionRepository = p;
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
                
                await _usuarioRepository.UpdateUserTokens(usuario.Id, 
                    new TokensUsuario(){AccessToken = accessToken, RefreshToken = refreshToken});
                
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, 
                    new {
                        id = usuario.Id,
                        name = usuario.Nome,
                        email = usuario.Email,
                        role = usuario.Cargo,
                        avatar = usuario.ImagemUrl,
                        access_token = accessToken,
                        refreshToken = refreshToken,
                    }
                ));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message + ex.StackTrace);
            }
        }
            
        public async Task<IActionResult> RegisterUser(RequestUsuario request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Nome) ||
                    string.IsNullOrWhiteSpace(request.Sobrenome) ||
                    string.IsNullOrWhiteSpace(request.Telefone) ||
                    string.IsNullOrWhiteSpace(request.Curso))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                }
                
                if (!IsValidEmail(request.Email))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Formato de email inválido!");
                }

                if (!IsValidPassword(request.Senha))
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, 
                        "Senha muito fraca! A senha deve ter ao menos 8 caracteres, " +
                        "uma letra maiúscula, uma letra minúscula e um número!");
                }
                

                var usuario = new Usuario
                {
                    Nome = request.Nome,
                    Sobrenome = request.Sobrenome,
                    Ra = request.Ra,
                    Curso = request.Curso,
                    Email = request.Email,
                    Telefone = request.Telefone,
                    SenhaHash = request.Senha,
                    Cargo = CargoUsuario.Aluno
                };

                await _usuarioRepository.CreateUser(usuario);

                usuario.SenhaHash = "shhhh";

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new { usuario }));
            }
            catch (InvalidConstraintException ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(
                    "Ocorreu um erro ao tentar cadastrar o usuário, favor relatar ao suporte pelo: contato.daccfei@gmail.com " +
                    ex.StackTrace);
            }
        }

        public async Task<IActionResult> RefreshUserToken(string refreshToken)
        {
            try
            {
                var userId = int.Parse(new JwtSecurityToken(refreshToken).Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var user = await _usuarioRepository.GetUserById(userId);
                
                var validRefreshToken = await _tokenService.ValidateRefreshToken(userId, refreshToken);
                if (!validRefreshToken || user == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.AUTH_TOKEN_INVALID);
                }
                
                var userPermissions = await _permissionRepository.GetPermissionsForRoleAsync(user.Cargo);
                
                var newAccessToken = _tokenService.GenerateAccessToken(user, userPermissions);
                var newRefreshToken = _tokenService.GenerateRefreshToken(user);
                
                var userTokens = new TokensUsuario(){ AccessToken = newAccessToken, RefreshToken = newRefreshToken };

                await _usuarioRepository.UpdateUserTokens(user.Id, userTokens);
                
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { userTokens }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message + ex.StackTrace);;
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
