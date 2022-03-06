using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Threading.Tasks;

namespace FileManager.IRepository
{
    public interface IAwsS3Service
    {
        Task<byte[]> DownloadFileAsync(string file);
        Task<string> UploadFileAsync(IFormFile file);
        Task<string> UploadFileAsync(Bitmap file, string name);
        Task<string> UpdateFileAsync(string oldName, string newName);
    }
}
