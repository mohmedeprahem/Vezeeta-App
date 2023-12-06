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
using Microsoft.IdentityModel.Tokens;

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
        public async Task<int> GetUsersCountByRole(string role, string lastDate = "")
        {
            int count = 0;
            string? roleId = _appDbContext
                .Roles
                .FirstOrDefault(r => r.Name == RolesEnum.Patient.ToString())
                ?.Id;

            // Get last date or current date
            if (!lastDate.IsNullOrEmpty())
            {
                DateTime lastWeekStartDate = lastDate switch
                {
                    "day" => DateTime.UtcNow.AddDays(-1),
                    "week" => DateTime.UtcNow.AddDays(-7),
                    "month" => DateTime.UtcNow.AddDays(-30),
                    "year" => DateTime.UtcNow.AddDays(-365),
                    _ => DateTime.UtcNow.AddDays(-7)
                };
                count = await _appDbContext
                    .Users
                    .Join(
                        _appDbContext.UserRoles,
                        user => user.Id,
                        userRole => userRole.UserId,
                        (user, userRole) => new { User = user, UserRole = userRole }
                    )
                    .Where(
                        joinResult =>
                            joinResult.UserRole.RoleId == roleId
                            && joinResult.User.CreatedAt >= lastWeekStartDate
                    )
                    .Select(joinResult => joinResult.User.Id)
                    .CountAsync();
            }
            else
            {
                count = await _appDbContext
                    .UserRoles
                    .CountAsync(userRole => userRole.RoleId == roleId);
            }

            return count;
        }
    }
}
