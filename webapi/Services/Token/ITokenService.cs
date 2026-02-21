using DaccApi.Model;

namespace DaccApi.Services.Token
{
    public interface ITokenService
    {
        string GenerateAccessToken(Usuario usuario, HashSet<string> permissions);
        string GenerateRefreshToken(Usuario usuario);
        Task<bool> ValidateRefreshToken(Guid userId, string refreshToken);
        
        // Métodos de Reset de Senha
        string GenerateResetToken();
        bool IsResetTokenValid(UsuarioResetToken token);
    }
}
