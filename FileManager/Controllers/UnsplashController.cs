using FileManager.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace FileManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnsplashController : ControllerBase
    {

        private readonly IUnsplashService _unsplashService;
        private readonly ILogger<UnsplashController> _logger;

        public UnsplashController(IUnsplashService unsplashService, ILogger<UnsplashController> logger)
        {
            _unsplashService = unsplashService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("search/{param}")]
        public async Task<IActionResult> GetPhotosUnsplash(string param)
        {
            try
            {
                var result = await _unsplashService.SearchPhotos(param);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("upload/{photoId}")]
        public async Task<IActionResult> UploadPhotoUnsplash(string photoId)
        {
            try
            {
                var result = await _unsplashService.uploadPhoto(photoId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
