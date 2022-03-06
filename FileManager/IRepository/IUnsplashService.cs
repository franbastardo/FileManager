using FileManager.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileManager.IRepository
{
    public interface IUnsplashService
    {
        Task<string> uploadPhoto(string photoId);
        Task<ImageDTO> SearchPhotos(string parameter);
    }
}
