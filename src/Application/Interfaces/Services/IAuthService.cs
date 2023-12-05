using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Core.Authentications;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IAuthService
    {
        public Task<IdentityResult> Register(RegisterDto model);
        public Task<AuthenticationResult> Login(LoginDto model);
    }
}
