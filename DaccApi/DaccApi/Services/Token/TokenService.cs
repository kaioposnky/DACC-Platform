using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DaccApi.Model;
using Microsoft.IdentityModel.Tokens;

namespace DaccApi.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Usuario usuario)
        {
            var expirationTime = DateTime.UtcNow.AddHours(3);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"]; 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new []
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            
            var tokenOptions = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: expirationTime,
                    signingCredentials: credentials
            );

            return _tokenHandler.WriteToken(tokenOptions);
        }
        
        public string RefreshToken(Usuario usuario, string token)
        {
            var jwtSecurityToken = _tokenHandler.ReadToken(token) as JwtSecurityToken;
            
            var userId = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (userId != usuario.Id.ToString())
            {
                throw new SecurityTokenException("Token não pertence ao usuário!");
            }
            
            // Se o token expirou gerar um novo
            if (jwtSecurityToken.ValidTo < DateTime.UtcNow)
            {
                return GenerateToken(usuario);
            }
            
            // Se o token não expirou retornar o mesmo
            return token;
        }
    }
}