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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly AppDbContext _appDbContext;

        public UserRepository(UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        // Get all patients
        public async Task<List<ApplicationUser>> GetUsersByRole(
            string role,
            int page = 1,
            int size = 8,
            string search = ""
        )
        {
            IList<ApplicationUser> usersInRole = await _userManager.GetUsersInRoleAsync(role);
            List<ApplicationUser> pagedUsers = usersInRole
                .Where(
                    user =>
                        user.FullName.Contains(search)
                        || user.Email.Contains(search)
                        || user.PhoneNumber.Contains(search)
                )
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
            return pagedUsers;
        }

        // Get Counts of all patients
        public async Task<int> GetUsersCountByRole(string role)
        {
            string? roleId = _appDbContext
                .Roles
                .FirstOrDefault(r => r.Name == RolesEnum.Patient.ToString())
                ?.Id;

            if (roleId == null)
            {
                return 0;
            }

            return await _appDbContext.UserRoles.CountAsync(ur => ur.RoleId == roleId);
        }
    }
}
