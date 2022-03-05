using FileManager.IRepository;
using BCrypt.Net;

namespace FileManager.Repository
{
    public class Encrypting : IEncrypting
    {
        public bool ComparePasswords(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, PasswordSalt());
        }

        public string PasswordSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }
    }
} 
