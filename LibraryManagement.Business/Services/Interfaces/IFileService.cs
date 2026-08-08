using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folder, string[]? allowedExtensions = null);
        Task<(byte[] FileContents, string ContentType, string FileName)> DownloadFileAsync(string folder ,string fileName);
    }
}
