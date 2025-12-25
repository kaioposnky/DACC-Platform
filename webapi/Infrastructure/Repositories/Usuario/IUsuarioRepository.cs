using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, Usuario entity);
        Task<bool> DeleteAsync(Guid id);

        // Métodos específicos
        Task<Guid> CreateUser(Usuario request);
        Task<Usuario?> GetUserByEmail(string email);
        Task<Usuario?> GetUserByRa(string ra);
        Task<TokensUsuario?> GetUserTokens(Guid id);
        Task UpdateUserTokens(Guid id, TokensUsuario tokensUsuario);
        Task<EstatisticasUsuario?> GetUserStats(Guid id);
        
        // Métodos de Reset de Senha
        Task SaveResetTokenAsync(UsuarioResetToken token);
        Task<UsuarioResetToken?> GetResetTokenAsync(string token);
        Task InvalidateResetTokenAsync(Guid tokenId);
        Task UpdatePasswordAsync(Guid userId, string newPasswordHash);
    }
}
