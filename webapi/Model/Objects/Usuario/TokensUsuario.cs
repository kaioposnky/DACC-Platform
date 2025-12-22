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
        public required string AccessToken { get; set; }
        /// <summary>
        /// Obtém ou define o token de atualização (refresh token).
        /// </summary>
        public required string RefreshToken { get; set; }

        /// <summary>
        /// Tempo em que o token vai expirar, unidade em Unix
        /// </summary>
        public required long ExpiresIn { get; set; }
    }
}