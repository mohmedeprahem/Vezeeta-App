using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.enums;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            this._doctorRepository = doctorRepository;
        }

        public async Task<List<ApplicationUser>> GetDoctors(int page, int size, string search)
        {
            return await _doctorRepository.GetDoctors(
                page = 1,
                size = 8,
                search = "",
                new string[] { "Specialization" }
            );
        }

        public async Task<int> GetDoctorsCount(string lastDate = "")
        {
            return await _doctorRepository.GetUsersCountByRole(
                RolesEnum.Doctor.ToString(),
                lastDate
            );
        }

        public async Task<ApplicationUser> GetPatientById(string id)
        {
            return await _doctorRepository.GetUserById(id);
        }
    }
}
