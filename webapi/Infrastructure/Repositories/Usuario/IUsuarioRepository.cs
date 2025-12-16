using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.User
{
    /// <summary>
    /// Define a interface para o repositório de usuários.
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        public Task CreateUser(Usuario request);
        /// <summary>
        /// Obtém um usuário específico pelo seu ID.
        /// </summary>
        public Task<Usuario?> GetUserById(Guid id);
        /// <summary>
        /// Obtém um usuário específico pelo seu e-mail.
        /// </summary>
        public Task<Usuario?> GetUserByEmail(string email);
        /// <summary>
        /// Obtém os tokens de um usuário.
        /// </summary>
        public Task<TokensUsuario?> GetUserTokens(Guid id);
        /// <summary>
        /// Atualiza os tokens de um usuário.
        /// </summary>
        public Task UpdateUserTokens(Guid id, TokensUsuario tokensUsuario);
        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        public Task<int> UpdateUser(Usuario user);
        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        public Task<List<Usuario>> GetAll();
        /// <summary>
        /// Deleta um usuário existente.
        /// </summary>
        public Task<int> DeleteUser(Guid id);

    }
}
