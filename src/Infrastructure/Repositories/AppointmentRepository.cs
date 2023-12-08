using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces.Repositories;
using Core.enums;
using Core.Models;
using Infrastructure.DataBase.Context;
using Infrastructure.Helpers.GeneralFunctions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly HelperFunctions _helperFunctions;

        public AppointmentRepository(AppDbContext appDbContext, HelperFunctions helperFunctions)
        {
            this._appDbContext = appDbContext;
            this._helperFunctions = helperFunctions;
        }

        public async Task<Appointment> CreateAppointmentDayAsync(Appointment appointment)
        {
            // get day as string
            DaysEnum dayEnum = (DaysEnum)appointment.DayId;

            appointment.Date = _helperFunctions.GetNextWeekday(dayEnum.ToString());
            await _appDbContext.Appointments.AddAsync(appointment);
            return appointment;
        }

        public async Task<AppointmentTime> CreateAppointmentTimeAsync(
            int appointmentId,
            TimeOnly time
        )
        {
            // Get time id
            Time? timeResult = await _appDbContext
                .Times
                .FirstOrDefaultAsync(t => t.TimeValue == time);

            if (timeResult == null)
            {
                timeResult = new Time { TimeValue = time };
                await _appDbContext.Times.AddAsync(timeResult);
                await _appDbContext.SaveChangesAsync();
            }
            AppointmentTime newAppointmentDay = new AppointmentTime
            {
                AppointmentId = appointmentId,
                TimeId = timeResult.Id
            };
            await _appDbContext.AppointmentTimes.AddAsync(newAppointmentDay);
            return newAppointmentDay;
        }

        public async Task<Appointment> GetAppointmentByDayIdAsync(int dayId, string doctorId)
        {
            Appointment? result = await _appDbContext
                .Appointments
                .FirstOrDefaultAsync(a => a.DayId == dayId && a.DoctorId == doctorId);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<bool> IsDayAddedBefore(int dayId, string doctorId)
        {
            // get day as string
            DaysEnum dayEnum = (DaysEnum)dayId;

            DateTime nextWeekday = _helperFunctions.GetNextWeekday(dayEnum.ToString());

            return await _appDbContext
                .Appointments
                .AnyAsync(a => a.DayId == dayId && a.DoctorId == doctorId && a.Date == nextWeekday);
        }
    }
}
