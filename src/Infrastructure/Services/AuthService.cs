using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Repositories;
using Application.Services;
using AutoMapper;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;

        public AuthService(IMapper mapper, IAuthRepository authRepository)
        {
            _mapper = mapper;
            _authRepository = authRepository;
        }

        public async Task<IdentityResult> RegisterUser(RegisterDto model)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(model);

            var result = await _authRepository.Register(user, model.Password);

            return result;
        }
    }
}
