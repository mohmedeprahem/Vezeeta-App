using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Azure;
using Core.enums;
using Core.Models;
using Infrastructure.DataBase.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PatientRepository : UserRepository, IPatientRepository
    {
        public PatientRepository(
            UserManager<ApplicationUser> userManager,
            AppDbContext appDbContext
        )
            : base(userManager, appDbContext) { }

        public async Task<int> GetPatientCountByStringAsync(string search = "")
        {
            // Get role id
            IdentityRole? role = await _appDbContext
                .Roles
                .FirstOrDefaultAsync(r => r.Name == RolesEnum.Patient.ToString());
            return await _appDbContext
                .Users
                .Join(
                    _appDbContext.UserRoles,
                    user => user.Id,
                    userRole => userRole.UserId,
                    (user, userRole) => new { User = user, UserRole = userRole }
                )
                .Where(
                    joined =>
                        joined.UserRole.RoleId == role.Id
                        && (
                            joined.User.FullName.Contains(search)
                            || joined.User.Email.Contains(search)
                            || joined.User.PhoneNumber.Contains(search)
                        )
                )
                .CountAsync();
        }
    }
}
