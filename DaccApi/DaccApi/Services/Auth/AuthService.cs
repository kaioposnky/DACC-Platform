using DaccApi.Model;
using DaccApi.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DaccApi.Helpers;
using DaccApi.Infrastructure.Cryptography;
using DaccApi.Infrastructure.Repositories.User;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IArgon2Utility _argon2Utility;
        public AuthService(IUsuarioRepository usuarioRepository, IArgon2Utility argon2Utility)
        {
            _usuarioRepository = usuarioRepository;
            _argon2Utility = argon2Utility;
        }
        
        public string GenerateToken(object request)
        {
            var expirationTime = DateTime.UtcNow.AddHours(3);

            var claims = new ClaimsIdentity();

            var properties = request.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(request)?.ToString();
                if (value != null)
                {
                    claims.AddClaim(new Claim(property.Name, value));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key.secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expirationTime,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // TODO: Depois dar uma olhada se essa implementação está razoável para o controller, apenas fiz a lógica
        public bool ValidateCredentials(RequestUsuario request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
            {
                return false;
            }

            var usuario = _usuarioRepository.GetUserByEmail(request.Email).Result;

            if (usuario == null)
            {
                return false;
            }
            
            return _argon2Utility.VerifyPassword(request.Senha, usuario.Senha);

        }
    }
}
