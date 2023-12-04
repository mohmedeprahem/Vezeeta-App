using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private UserManager<IdentityUser> _userManager;

        public AuthRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Register(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
    }
}
