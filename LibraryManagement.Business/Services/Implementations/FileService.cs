using LibraryManagement.Business.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private const long MaxFileSize = 5 * 1024 * 1024; 

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folder, string[]? allowedExtensions = null)
        {
            if(file==null || file.Length==0)
                throw new ValidationException("File is not selected or empty.");

            if(file.Length > MaxFileSize)
                throw new ValidationException("File size is too large. Maximum allowed size is 5MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (allowedExtensions != null && allowedExtensions.Length >0)
            {
                if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                    throw new ValidationException($"This file type is not allowed. Allowed extensions: {string.Join(", ", allowedExtensions)}");
            }

            var rootPath = _environment.WebRootPath ?? _environment.ContentRootPath;
            var folderPath = Path.Combine(rootPath, folder);

            if(!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using(var stream = new FileStream(filePath , FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }

        public async Task<(byte[] FileContents, string ContentType, string FileName)> DownloadFileAsync(string folder, string fileName)
        {
            var rootPath = _environment.WebRootPath ?? _environment.ContentRootPath;
            var filePath = Path.Combine(rootPath, folder);

            if(!File.Exists(filePath))
                    throw new FileNotFoundException("The requested file was not found.");

            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var contentType = GetMimeType(filePath);

            return (fileBytes, contentType ,fileName);
        }

        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream",
            };
        }
    }
}
