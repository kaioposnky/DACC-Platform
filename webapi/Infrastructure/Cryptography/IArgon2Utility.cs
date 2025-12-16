namespace DaccApi.Infrastructure.Cryptography
{
    /// <summary>
    /// Define a interface para utilitários de criptografia com Argon2.
    /// </summary>
    public interface IArgon2Utility
    {
        /// <summary>
        /// Gera o hash de uma senha.
        /// </summary>
        public string HashPassword(string password);
        /// <summary>
        /// Verifica se uma senha corresponde a um hash.
        /// </summary>
        public bool VerifyPassword(string password, string hashedPassword);
        /// <summary>
        /// Gera um salt criptográfico.
        /// </summary>
        public byte[] GenerateSalt();
    }
}
