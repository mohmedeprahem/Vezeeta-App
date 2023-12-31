﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Core.enums;
using Core.Models;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IFileService fileService,
            IMapper mapper
        )
        {
            this._doctorRepository = doctorRepository;
            this._fileService = fileService;
            this._mapper = mapper;
        }

        public async Task<List<ApplicationUser>> GetDoctors(
            int page,
            int size,
            string search,
            string[] includes = null
        )
        {
            return await _doctorRepository.GetDoctors(page, size, search, includes);
        }

        public async Task<int> GetDoctorsCount(string lastDate = "")
        {
            return await _doctorRepository.GetUsersCountByRole(
                RolesEnum.Doctor.ToString(),
                lastDate
            );
        }

        public async Task<ApplicationUser> GetDoctorById(string id)
        {
            return await _doctorRepository.GetDoctorById(id, new string[] { "Specialization" });
        }

        public async Task<IdentityResult> UpdateDoctor(string doctorId, UpdateDoctorDto model)
        {
            string savePath = string.Empty;
            string imageName = string.Empty;
            try
            {
                ApplicationUser user = _mapper.Map<ApplicationUser>(model);

                if (model.Image != null)
                {
                    // save image
                    savePath = Path.Combine(Directory.GetCurrentDirectory(), "../../uploads");
                    imageName = await _fileService.UploadFile(model.Image, savePath);
                    user.Image = imageName;
                }
                user.Id = doctorId;

                IdentityResult identityResult = await _doctorRepository.UpdateDoctor(user);
                string imagePath = string.Empty;

                if (model.Image != null)
                {
                    if (!identityResult.Succeeded)
                    {
                        // Delete new image if failed
                        imagePath = Path.Combine(savePath, imageName);
                    }
                    else
                    {
                        // Delete old image if updated
                        imagePath = Path.Combine(savePath, model.OldImage);
                    }

                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                return identityResult;
            }
            catch (Exception ex)
            {
                string imagePath = Path.Combine(savePath, imageName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }

                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetDoctorsCountByString(string search = "")
        {
            return await _doctorRepository.GetDoctorCountByString(search);
        }

        public async Task<IdentityResult> DeleteDoctor(string userId)
        {
            ApplicationUser doctor = await _doctorRepository.GetDoctorById(
                userId,
                ["Appointments", "Appointments.Times", "Appointments.Times.Booking"]
            );
            if (doctor == null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "NotFound" });
            }

            if (doctor.Appointments != null)
            {
                foreach (var appointment in doctor.Appointments)
                {
                    if (appointment.Times != null)
                    {
                        foreach (var time in appointment.Times)
                        {
                            if (time.Booking != null)
                            {
                                return IdentityResult.Failed(
                                    new IdentityError { Code = "NotAuthorized" }
                                );
                            }
                        }
                    }
                }
            }

            await _doctorRepository.DeleteDoctor(doctor);
            return IdentityResult.Success;
        }
    }
}
