using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories;
using Core.enums;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<IdentityResult> Register(ApplicationUser user, string password)
        {
            try
            {
                user.UserName = user.Email;
                IdentityResult resultStatus = await _userManager.CreateAsync(user, password);
                if (resultStatus.Succeeded)
                {
                    IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(
                        user,
                        RolesEnum.Patient.ToString()
                    );
                }

                return resultStatus;
            }
            catch (InvalidOperationException ex)
            {
                await _userManager.DeleteAsync(user);
                throw new Exception(ex.Message);
            }
        }
    }
}
