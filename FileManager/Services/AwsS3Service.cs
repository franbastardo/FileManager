using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using FileManager.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace FileManager.Services
{
    public class AwsS3Service : IAwsS3Service
    {
        private readonly IAmazonS3 _awsS3Client;
        private readonly string _bucketName ;
		private readonly ILogger<AwsS3Service> _logger;
        public AwsS3Service(ILogger<AwsS3Service> logger)
        {
            _awsS3Client = new AmazonS3Client(
            Environment.GetEnvironmentVariable("S3KEY"), 
            Environment.GetEnvironmentVariable("S3SECRET"),
			Amazon.RegionEndpoint.EUWest1);
			_bucketName = Environment.GetEnvironmentVariable("S3BUCKETNAME");
			_logger = logger;

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
					_logger.LogInformation(string.Format("The document '{0}' is not found", file));

				return ms.ToArray();
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Something went wrong with {ex}");
				return null;
			}
		}

        public async Task<string> UpdateFileAsync(string oldName, string newName)
        {
			try
			{
				var copy = new CopyObjectRequest();
				copy.SourceBucket = _bucketName;
				copy.SourceKey = oldName;
				copy.DestinationBucket = _bucketName;
				copy.DestinationKey = newName;
				await _awsS3Client.CopyObjectAsync(copy);

                var delete = new DeleteObjectRequest();
                delete.Key = oldName;
                delete.BucketName = _bucketName;
				await _awsS3Client.DeleteObjectAsync(delete);

                return $"https://{_bucketName}.{Amazon.RegionEndpoint.EUWest3}.s3.amazonaws.com/{newName}";
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Something went wrong with {ex}");
				return null;
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

					return  $"https://{_bucketName}.{Amazon.RegionEndpoint.EUWest3}.s3.amazonaws.com/{file.FileName}"; 
				}
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Something went wrong with {ex}");
				return null;
			}
		}

		public async Task<string> UploadFileAsync(Bitmap file, string name)
		{
			try
			{
				using (var newMemoryStream = new MemoryStream())
				{
					file.Save(newMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

					var uploadRequest = new TransferUtilityUploadRequest
					{
						InputStream = newMemoryStream,
						Key = name,
						BucketName = _bucketName,
						ContentType = "binary/octet-stream"
					};

					var fileTransferUtility = new TransferUtility(_awsS3Client);

					await fileTransferUtility.UploadAsync(uploadRequest);

					return $"https://{_bucketName}.{Amazon.RegionEndpoint.EUWest3}.s3.amazonaws.com/{name}"; ;
				}
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Something went wrong with {ex}");
				return null;
			}
		}
	}
}
