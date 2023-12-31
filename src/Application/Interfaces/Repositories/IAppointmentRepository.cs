﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        public Task<Appointment> CreateAppointmentDayAsync(Appointment appointment);
        public Task<Appointment> GetAppointmentByDayIdAsync(int dayId, string doctorId);
        public Task<AppointmentTime> CreateAppointmentTimeAsync(
            int appointmentDayId,
            TimeOnly time
        );
        public Task<bool> IsDayAddedBefore(int dayId, string doctorId);
        public Task<AppointmentTime> GetAppointmentTimeById(int id, string[] includes = null);
        public Task<IdentityResult> ChangeAppointmentTimeStatus(int id);
        public Task<IdentityResult> UpdateAppointmentTimeAsync(
            int appointmentTimeId,
            TimeOnly time
        );
        public Task<IdentityResult> DeleteAppointmentTimeAsync(int id);
    }
}
