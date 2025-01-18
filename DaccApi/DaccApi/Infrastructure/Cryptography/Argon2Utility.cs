namespace DaccApi.Infrastructure.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Konscious.Security.Cryptography;

    public class Argon2Utility : IArgon2Utility
    {
        private const int SaltSize = 16; 
        private const int HashSize = 32; 
        private const int Iterations = 4; 
        private const int MemorySize = 65536; 

        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

            byte[] salt = GenerateSalt();

            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                Iterations = Iterations,
                MemorySize = MemorySize,
                DegreeOfParallelism = 4 
            };

            byte[] hash = argon2.GetBytes(HashSize);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
                throw new ArgumentNullException("Password and hashedPassword cannot be null or empty.");

            var parts = hashedPassword.Split('.');
            if (parts.Length != 2)
                throw new FormatException("Invalid hash format.");

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hashStored = Convert.FromBase64String(parts[1]);

            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                Iterations = Iterations,
                MemorySize = MemorySize,
                DegreeOfParallelism = 4 
            };

            byte[] hashComputed = argon2.GetBytes(HashSize);

            return CryptographicOperations.FixedTimeEquals(hashStored, hashComputed);
        }
        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(salt);
            return salt;
        }
    }

}
