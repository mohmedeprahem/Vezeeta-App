using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Azure;
using Core.enums;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository;
        }

        public async Task<List<ApplicationUser>> GetPatients(int page, int size)
        {
            return await _patientRepository.GetUsersByRole(
                RolesEnum.Patient.ToString(),
                page,
                size
            );
        }

        public async Task<int> GetPatientsCount()
        {
            return await _patientRepository.GetUsersCountByRole(RolesEnum.Patient.ToString());
        }
    }
}
