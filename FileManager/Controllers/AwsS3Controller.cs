using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FileManager.IRepository;
using System.Net;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using FileManager.DTOs;

namespace FileManager.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class AwsS3Controller : ControllerBase
	{
		private readonly IAwsS3Service _awsS3Service;
		private readonly ILogger<AwsS3Controller> _logger;

		public AwsS3Controller(IAwsS3Service awsS3Service, ILogger<AwsS3Controller> logger)
		{
			_awsS3Service = awsS3Service;
			_logger = logger;
		}

		[Authorize]
		[HttpGet("{documentName}")]
		public async Task<IActionResult> GetDocFromS3(string documentName)
		{
			try
			{
				if (string.IsNullOrEmpty(documentName))
					return BadRequest("The document name is required");

				var document = await _awsS3Service.DownloadFileAsync(documentName);

				return File(document, "application/octet-stream", documentName);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Something went Wrong in the {nameof(AwsS3Controller)}");
				return StatusCode(500, "Internal Server Error");
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> UploadDocumentToS3(IFormFile file)
		{
			try
			{
				if (file is null || file.Length <= 0)
					return BadRequest("file is required to upload");

				var result = await _awsS3Service.UploadFileAsync(file);
				_logger.LogInformation("****************");
				_logger.LogInformation(result);

				return Ok("successfully upload");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Something went Wrong in the {nameof(AwsS3Controller)}");
				return StatusCode(500, "Internal Server Error");
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> UpdateDocumentName([FromBody] UpdateFileDTO updateFileDTO)
		{
			if (!ModelState.IsValid)
			{
				_logger.LogInformation($"Missing parameters {updateFileDTO.NewName} or  {updateFileDTO.OldName}");
				return BadRequest(ModelState);
			}
			try
			{
				var result = await _awsS3Service.UpdateFileAsync(updateFileDTO.OldName,updateFileDTO.NewName);
				_logger.LogInformation(result);

				return Ok("successfully updatep");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Something went Wrong in the {nameof(AwsS3Controller)}");
				return StatusCode(500, "Internal Server Error");
			}
		}
	}
}
