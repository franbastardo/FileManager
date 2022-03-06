using FileManager.DTOs;
using FileManager.IRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FileManager.Services
{
    public class UnsplashService : IUnsplashService
    {
        
        public async Task<ImageDTO> SearchPhotos(string parameter)
        {
            var client = new HttpClient();
            var unsplashClientId = Environment.GetEnvironmentVariable("ACCESSKEY");
            var result = await client.GetAsync($"https://api.unsplash.com/search/photos?query={parameter}&client_id={unsplashClientId}");
            var responseContent = result.Content.ReadAsStringAsync().Result;
            var Images = JsonConvert.DeserializeObject<ImageDTO>(responseContent);

            return Images;

        }

        public Task<string> uploadPhoto()
        {
            throw new System.NotImplementedException();
        }
    }
}
