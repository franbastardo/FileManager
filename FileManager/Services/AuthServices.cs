using FileManager.Data;

namespace FileManager.Services
{
    public class AuthServices
    {
                
        private readonly FileManagerContext _context;

        public AuthServices(FileManagerContext congtext)
        {
            _context = congtext;
        }


    }
}
