using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(Guid id, Usuario entity);
        Task<bool> DeleteAsync(Guid id);

        // Métodos específicos mantidos
        public Task CreateUser(Usuario request);
        public Task<Usuario?> GetUserByEmail(string email);
        public Task<TokensUsuario?> GetUserTokens(Guid id);
        public Task UpdateUserTokens(Guid id, TokensUsuario tokensUsuario);
    }
}
