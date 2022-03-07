using System.Threading.Tasks;

namespace FileManager.IRepository
{
    public interface ISendGridService
    {
        Task<bool> SendEmail(string token, string email);
    }
}
