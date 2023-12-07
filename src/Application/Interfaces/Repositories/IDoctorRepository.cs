using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Repositories
{
    public interface IDoctorRepository : IUserRepository
    {
        public Task<List<ApplicationUser>> GetDoctors(
            int page,
            int size,
            string search = "",
            string[] includes = null
        );

        public Task<ApplicationUser> GetDoctorById(string id, string[] includes = null);
        public Task<IdentityResult> UpdateDoctor(ApplicationUser doctor);
    }
}
