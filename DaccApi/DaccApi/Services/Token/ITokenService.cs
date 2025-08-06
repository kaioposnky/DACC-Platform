using DaccApi.Model;

namespace DaccApi.Services.Token
{
    public interface ITokenService
    {
        public string GenerateAccessToken(Usuario usuario, HashSet<string> permissions);
        public string GenerateRefreshToken(Usuario usuario);
        public Task<bool> ValidateRefreshToken(Guid userId, string refreshToken);
    }
}