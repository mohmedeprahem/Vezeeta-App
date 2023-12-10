using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Services
{
    public interface IDoctorService
    {
        public Task<List<ApplicationUser>> GetDoctors(
            int page,
            int size,
            string search,
            string[] includes = null
        );
        public Task<int> GetDoctorsCount(string lastDate = "");
        public Task<ApplicationUser> GetDoctorById(string id);
        public Task<IdentityResult> UpdateDoctor(string doctorId, UpdateDoctorDto model);
        public Task<int> GetDoctorsCountByString(string search = "");
    }
}
