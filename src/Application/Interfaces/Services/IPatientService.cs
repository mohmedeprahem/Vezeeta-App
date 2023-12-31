﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Core.Models;

namespace Application.Interfaces.Services
{
    public interface IPatientService
    {
        public Task<List<ApplicationUser>> GetPatients(int page, int size, string search);
        public Task<int> GetPatientsCount(string lastDate = "");
        public Task<PatientAllInfoDto> GetAllInfoPatientById(string id);
        public Task<int> GetPatientCountByStringAsync(string search = "");
    }
}
