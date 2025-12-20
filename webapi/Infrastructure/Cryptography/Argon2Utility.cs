namespace DaccApi.Infrastructure.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Konscious.Security.Cryptography;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implementação do utilitário de criptografia Argon2 para hashing de senhas.
    /// </summary>
    public class Argon2Utility : IArgon2Utility
    {
        private const int SaltSize = 16; 
        private const int HashSize = 32; 
        private const int Iterations = 4; 
        private const int MemorySize = 65536; 
        private const int DegreeOfParallelism = 4;

        /// <summary>
        /// Gera o hash de uma senha usando Argon2id.
        /// </summary>
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

            var salt = GenerateSalt();

            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                Iterations = Iterations,
                MemorySize = MemorySize,
                DegreeOfParallelism = DegreeOfParallelism
            };

            var hash = argon2.GetBytes(HashSize);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        /// <summary>
        /// Verifica se uma senha fornecida corresponde a um hash existente.
        /// </summary>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                throw new ArgumentException("Password and hashedPassword cannot be null or empty.");

            try
            {
                var parts = hashedPassword.Split('.');
                if (parts.Length != 2)
                    return false;

                var salt = Convert.FromBase64String(parts[0]);
                var hashStored = Convert.FromBase64String(parts[1]);

                using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    Iterations = Iterations,
                    MemorySize = MemorySize,
                    DegreeOfParallelism = DegreeOfParallelism
                };

                var hashComputed = argon2.GetBytes(HashSize);

                return CryptographicOperations.FixedTimeEquals(hashStored, hashComputed);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gera um salt criptograficamente seguro.
        /// </summary>
        public byte[] GenerateSalt()
        {
            var salt = new byte[SaltSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }
    }
}
