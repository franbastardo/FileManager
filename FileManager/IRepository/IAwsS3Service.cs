using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileManager.IRepository
{
    public interface IAwsS3Service
    {
        Task<byte[]> DownloadFileAsync(string file);
        Task<string> UploadFileAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string fileName, string versionId = "");
    }
}
