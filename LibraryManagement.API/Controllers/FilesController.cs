using LibraryManagement.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".pdf" };

        public FilesController(IFileService fileService)
        {
            _fileService= fileService;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var fileName = await _fileService.UploadFileAsync(file, "Uploads", _allowedImageExtensions);

            return Ok(new
            {
                Message = "File uploaded successfully.",
                FileName = fileName
            });
        }

        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            var(fileBytes, contentType, downloadName) = await _fileService.DownloadFileAsync("Uploads", fileName);
            return File(fileBytes, contentType, downloadName);
        }
    }
}
