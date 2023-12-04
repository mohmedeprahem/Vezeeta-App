using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces.Services;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Core.Models;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;
        private readonly IFileService _fileService;

        public AuthService(IMapper mapper, IAuthRepository authRepository, IFileService fileService)
        {
            this._mapper = mapper;
            this._authRepository = authRepository;
            this._fileService = fileService;
        }

        public async Task<IdentityResult> Register(RegisterDto model)
        {
            string imageName = string.Empty;
            string SavePath = string.Empty;
            ApplicationUser user = _mapper.Map<ApplicationUser>(model);

            if (model.Image != null)
            {
                SavePath = Path.Combine(Directory.GetCurrentDirectory(), "../../uploads");
                imageName = await _fileService.UploadFile(model.Image, SavePath);
                user.Image = imageName;
            }

            IdentityResult result = await _authRepository.Register(user, model.Password);

            if (!result.Succeeded)
            {
                string imagePath = Path.Combine(SavePath, imageName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }
            return result;
        }
    }
}
