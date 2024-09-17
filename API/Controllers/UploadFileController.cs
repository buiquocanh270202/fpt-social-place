using Application.Queries.GetRelationship;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UploadFileController : BaseController
    {
        private readonly Cloudinary _cloudinary;

        public UploadFileController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadImage(string userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            string folderPath = $"img/{userId}";
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folderPath
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new
                {
                    uploadResult.PublicId,
                    uploadResult.Url
                });
            }
            else
            {
                return StatusCode((int)uploadResult.StatusCode, uploadResult.Error.Message);
            }
        }

        [HttpPost("uploadVideo")]
        public async Task<IActionResult> UploadVideo(string userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }
            string folderPath = $"video/{userId}";
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = folderPath
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new
                {
                    uploadResult.PublicId,
                    uploadResult.Url
                });
            }
            else
            {
                return StatusCode((int)uploadResult.StatusCode, uploadResult.Error.Message);
            }
        }

    }
}

