using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    public interface IUsuarioRepository
    {
        public Task CreateUser(Usuario request);
        public Task<Usuario?> GetUserById(int id);
        public Task<Usuario?> GetUserByEmail(string email);
        public Task<TokensUsuario?> GetUserTokens(int id);
        public Task UpdateUserTokens(int id, TokensUsuario tokensUsuario);
        public Task<int> UpdateUser(Usuario user);
        public Task<List<Usuario>> GetAll();
        public Task<int> DeleteUser(int id);

    }
}
