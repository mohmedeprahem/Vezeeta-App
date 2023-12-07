using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Helpers
{
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment hostingEnvironment;

        public FileService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> UploadFile(IFormFile file, string SavePath)
        {
            // Use a unique identifier (e.g., a GUID)
            string uniqueId = Guid.NewGuid().ToString();

            // Use a timestamp to ensure uniqueness
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            // Combine the unique identifier and timestamp
            string imageName = $"{uniqueId}_{timestamp}{Path.GetExtension(file.FileName)}";

            string filePath = Path.Combine(SavePath, imageName);

            using var stream = File.Create(filePath);

            await file.CopyToAsync(stream);

            return imageName;
        }
    }
}
