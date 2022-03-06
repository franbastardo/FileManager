using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FileManager.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FileManager.Services
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly IAmazonS3 _awsS3Client;
        private readonly string _bucketName ;
        public AwsS3Service()
        {
            _awsS3Client = new AmazonS3Client(
                Environment.GetEnvironmentVariable("S3KEY"), 
                Environment.GetEnvironmentVariable("S3SECRET"),
				Amazon.RegionEndpoint.EUWest1);
			_bucketName = Environment.GetEnvironmentVariable("S3BUCKETNAME");

		}

        public Task<bool> DeleteFileAsync(string fileName, string versionId = "")
        {
            throw new System.NotImplementedException();
        }

        public async Task<byte[]> DownloadFileAsync(string file)
        {
			MemoryStream ms = null;

			try
			{
				GetObjectRequest getObjectRequest = new GetObjectRequest
				{
					BucketName = _bucketName,
					Key = file
				};

				using (var response = await _awsS3Client.GetObjectAsync(getObjectRequest))
				{
					if (response.HttpStatusCode == HttpStatusCode.OK)
					{
						using (ms = new MemoryStream())
						{
							await response.ResponseStream.CopyToAsync(ms);
						}
					}
				}

				if (ms is null || ms.ToArray().Length < 1)
					throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));

				return ms.ToArray();
			}
			catch (Exception)
			{
				throw;
			}
		}

        public async Task<string> UploadFileAsync(IFormFile file)
        {
			try
			{
				using (var newMemoryStream = new MemoryStream())
				{
					file.CopyTo(newMemoryStream);

					var uploadRequest = new TransferUtilityUploadRequest
					{
						InputStream = newMemoryStream,
						Key = file.FileName,
						BucketName = _bucketName,
						ContentType = file.ContentType
					};

					var fileTransferUtility = new TransferUtility(_awsS3Client);

					await fileTransferUtility.UploadAsync(uploadRequest);

					return  $"https://{_bucketName}.{Amazon.RegionEndpoint.EUWest3}.s3.amazonaws.com/{file.FileName}"; ;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
    }
}
