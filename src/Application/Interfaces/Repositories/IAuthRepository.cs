using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Repositories
{
    public interface IAuthRepository
    {
        public Task<IdentityResult> Register(ApplicationUser user, string password);
    }
}
