using DaccApi.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DaccApi.Helpers;
using DaccApi.Infrastructure.Cryptography;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Services.Token;
using Helpers.Response;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IArgon2Utility _argon2Utility;
        private readonly ITokenService _tokenService;
        public AuthService(IUsuarioRepository usuarioRepository, IArgon2Utility argon2Utility, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _argon2Utility = argon2Utility;
            _tokenService = tokenService;
        }
        private bool ValidateCredentials(string email, string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            
            return _argon2Utility.VerifyPassword(password, hashedPassword);
        }
        public IActionResult LoginUser(RequestUsuario request)
        {
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
            
            var usuario = _usuarioRepository.GetUserByEmail(request.Email).Result;

            if (usuario == null || !ValidateCredentials(request.Email, request.Senha, usuario.SenhaHash))
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INVALID_CREDENTIALS);
            }

            var token = _tokenService.GenerateToken(usuario);
            
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, 
                new {
                    id = usuario.Id,
                    name = usuario.Nome,
                    email = usuario.Email,
                    role = usuario.TipoUsuario,
                    avatar = usuario.ImagemUrl,
                    token = token
                }
            ));
        }
    }
}
