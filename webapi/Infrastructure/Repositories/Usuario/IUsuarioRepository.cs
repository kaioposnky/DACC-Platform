using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface IUsuarioRepository
    {
        public Task CreateUser(Usuario request);
        public Task<Usuario?> GetUserById(Guid id);
        public Task<Usuario?> GetUserByEmail(string email);
        public Task<TokensUsuario?> GetUserTokens(Guid id);
        public Task UpdateUserTokens(Guid id, TokensUsuario tokensUsuario);
        public Task<int> UpdateUser(Usuario user);
        public Task<List<Usuario>> GetAll();
        public Task<int> DeleteUser(Guid id);

    }
}
