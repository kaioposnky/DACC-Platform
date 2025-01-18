using DaccApi.Model;
using DaccApi.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DaccApi.Services.Auth
{
    public class AuthService : IAuthService
    {
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

        // Simulação de validação de credenciais (essa parte pode ser adaptada para autenticar a partir de banco de dados)
        public bool ValidateCredentials(RequestUsuario request)
        {
            // Implementar a lógica de validação de usuário (e.g., comparar email e senha com o banco de dados)
            // Para fins de exemplo, vamos retornar true, assumindo que as credenciais são válidas.
            return true;
        }
    }
}
