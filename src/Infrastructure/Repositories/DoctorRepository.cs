using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Core.Models;
using Infrastructure.DataBase.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
    }
}
