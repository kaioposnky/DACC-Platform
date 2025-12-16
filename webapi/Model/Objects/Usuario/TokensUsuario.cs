namespace DaccApi.Model
{
    /// <summary>
    /// Representa os tokens de autenticação de um usuário.
    /// </summary>
    public class TokensUsuario
    {
        /// <summary>
        /// Obtém ou define o token de acesso.
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Obtém ou define o token de atualização (refresh token).
        /// </summary>
        public string RefreshToken { get; set; }
    }
}