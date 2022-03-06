using FileManager.DTOs;
using FileManager.IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FileManager.Services;
using System.Net;
using System.Drawing;
using Image = FileManager.DTOs.Image;

namespace FileManager.Services
{
    public class UnsplashService : IUnsplashService
    {
        private readonly IAwsS3Service _awsS3Service;

        public UnsplashService(IAwsS3Service awsS3Service)
        {
            _awsS3Service = awsS3Service;
        }

        public async Task<ImageDTO> SearchPhotos(string parameter)
        {
            var client = new HttpClient();
            var unsplashClientId = Environment.GetEnvironmentVariable("ACCESSKEY");
            var result = await client.GetAsync($"https://api.unsplash.com/search/photos?query={parameter}&client_id={unsplashClientId}");
            var responseContent = result.Content.ReadAsStringAsync().Result;
            var Images = JsonConvert.DeserializeObject<ImageDTO>(responseContent);

            return Images;

        }

        public async Task<string> uploadPhoto(string photoId)
        {
            var client = new HttpClient();
            var unsplashClientId = Environment.GetEnvironmentVariable("ACCESSKEY");
            var result = await client.GetAsync($"https://api.unsplash.com/photos/{photoId}?client_id={unsplashClientId}");
            var responseContent = result.Content.ReadAsStringAsync().Result;
            var Images = JsonConvert.DeserializeObject<Image>(responseContent);

            var webClient = new WebClient();
            var stream = webClient.OpenRead(Images.Links.Download);
            var bitmap = new Bitmap(stream);

            string fileLink = "";

            if (bitmap != null)
            {
                fileLink = await _awsS3Service.UploadFileAsync(bitmap, $"{Images.Id}.jpg");
            }
            stream.Flush();
            stream.Close();
            webClient.Dispose();
            
            return fileLink;
        }
    }
}
