namespace DaccApi.Infrastructure.Cryptography
{
    public interface IArgon2Utility
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hashedPassword);
        public byte[] GenerateSalt();
    }
}
