namespace FileManager.IRepository
{
    public interface IEncrypting
    {
        public string HashPassword( string password );
        public string PasswordSalt();
        public bool ComparePasswords( string password, string hash);
        
    }
}
