using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IFileService
    {
        public Task<string> UploadFile(IFormFile file, string SavePath);
    }
}
