using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using DaccApi.Infrastructure.Repositories.Permission;
using DaccApi.Infrastructure.Repositories.User;
using DaccApi.Model;
using Microsoft.IdentityModel.Tokens;

namespace DaccApi.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public TokenService(IConfiguration configuration, IUsuarioRepository usuarioRepository, IPermissionRepository permissionRepository)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
            _permissionRepository = permissionRepository;
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
        
        public async Task<bool> ValidateRefreshToken(int userId, string refreshToken)
        {
            var userTokensOld = await _usuarioRepository.GetUserTokens(userId);
            var oldRefeshToken = userTokensOld.RefreshToken;

            if (string.IsNullOrWhiteSpace(oldRefeshToken))
            {
                return true;
            }

            return oldRefeshToken.Equals(refreshToken);
        }
    }
}