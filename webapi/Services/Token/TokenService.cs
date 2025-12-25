using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model;
using Microsoft.IdentityModel.Tokens;

namespace DaccApi.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public TokenService(IConfiguration configuration, IUsuarioRepository usuarioRepository)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
        }

        public string GenerateAccessToken(Usuario usuario, HashSet<string> permissions)
        {
            var expirationTime = DateTime.UtcNow.AddMinutes(15);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"]; 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Cargo),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // adiciona as permissões do cargo
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }
            
            var tokenOptions = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: expirationTime,
                    signingCredentials: credentials
            );

            return _tokenHandler.WriteToken(tokenOptions);
        }

        public string GenerateRefreshToken(Usuario usuario)
        {
            var expirationTime = DateTime.UtcNow.AddHours(1);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"]; 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
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
        
        public async Task<bool> ValidateRefreshToken(Guid userId, string refreshToken)
        {
            var userTokensOld = await _usuarioRepository.GetUserTokens(userId);
            var oldRefeshToken = userTokensOld!.RefreshToken;
            
            var token = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);

            if (token.ValidTo < DateTime.UtcNow)
            {
                return false;
            }
            
            // se o token salvo for "" ele deu logout, então o token é inválido
            return !string.IsNullOrWhiteSpace(oldRefeshToken) || oldRefeshToken.Equals(refreshToken);
        }

        public string GenerateResetToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        }

        public bool IsResetTokenValid(UsuarioResetToken token)
        {
            if (token == null) return false;
            if (token.Usado) return false;
            if (token.DataExpiracao < DateTime.UtcNow) return false;

            return true;
        }
    }
}
