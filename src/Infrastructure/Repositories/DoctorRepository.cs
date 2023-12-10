using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Core.enums;
using Core.Models;
using Infrastructure.DataBase.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.Repositories
{
    public class DoctorRepository : UserRepository, IDoctorRepository
    {
        public DoctorRepository(UserManager<ApplicationUser> userManager, AppDbContext appDbContext)
            : base(userManager, appDbContext) { }

        public async Task<List<ApplicationUser>> GetDoctors(
            int page = 1,
            int size = 8,
            string search = "",
            string[] includes = null
        )
        {
            IQueryable<ApplicationUser> query = _appDbContext.Set<ApplicationUser>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            List<ApplicationUser> pagedUsers = await query
                .Where(
                    user =>
                        user.FullName.Contains(search)
                        || user.Email.Contains(search)
                        || user.PhoneNumber.Contains(search)
                )
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
            return pagedUsers;
        }

        public async Task<ApplicationUser> GetDoctorById(string id, string[] includes = null)
        {
            IQueryable<ApplicationUser> query = _appDbContext.Set<ApplicationUser>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<IdentityResult> UpdateDoctor(ApplicationUser doctor)
        {
            try
            {
                // Step 1: Retrieve the entity using LINQ
                ApplicationUser? entityToUpdate = _appDbContext
                    .Users
                    .FirstOrDefault(entity => entity.Id == doctor.Id);
                if (entityToUpdate != null)
                {
                    bool IsUserDoctor = await _userManager.IsInRoleAsync(
                        entityToUpdate,
                        RolesEnum.Doctor.ToString()
                    );
                    if (IsUserDoctor)
                    {
                        entityToUpdate.FirstName = doctor.FirstName;
                        entityToUpdate.LastName = doctor.LastName;
                        entityToUpdate.Email = doctor.Email;
                        entityToUpdate.PhoneNumber = doctor.PhoneNumber;
                        entityToUpdate.SpecializationId = doctor.SpecializationId;
                        entityToUpdate.Gender = doctor.Gender;
                        entityToUpdate.DateOfBirth = doctor.DateOfBirth;
                        if (doctor.Image != null)
                            entityToUpdate.Image = doctor.Image;

                        _appDbContext.SaveChanges();
                        return IdentityResult.Success;
                    }
                }
                return IdentityResult.Failed();
            }
            catch (DbUpdateException ex)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<int> GetDoctorCountByString(string search = "")
        {
            // Get role id
            IdentityRole? role = await _appDbContext
                .Roles
                .FirstOrDefaultAsync(r => r.Name == "Doctor");
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
