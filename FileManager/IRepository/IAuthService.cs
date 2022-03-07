using FileManager.DTOs;
using System.Threading.Tasks;

namespace FileManager.IRepository
{
    public interface IAuthService
    {
        Task<string> BuildToken(string email);
        Task<string> ValidateToken(string token);
    }
}
