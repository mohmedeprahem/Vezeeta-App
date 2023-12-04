using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public interface IAuthService
    {
        public Task<IdentityResult> RegisterUser(RegisterDto model);
    }
}
