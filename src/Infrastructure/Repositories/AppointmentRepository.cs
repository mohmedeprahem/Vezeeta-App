using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _appDbContext.Appointments.Add(appointment);
            return appointment;
        }

        public Task<AppointmentTime> CreateAppointmentTimeAsync(int appointmentDayId, string time)
        {
            throw new NotImplementedException();
        }

        /*        public async Task<AppointmentTime> CreateAppointmentTimeAsync(
                    string appointmentId,
                    string time
                )
                {
                    // Get time id
                    await _appDbContext.Times.FirstOrDefault(t => t.TimeValue == time);
                    await _appDbContext
                        .AppointmentTimes
                        .AddAsync(new AppointmentTime { AppointmentId = appointmentId, Time = time });
                }*/

        public async Task<Appointment> GetAppointmentByDayIdAsync(int dayId, string doctorId)
        {
            return await _appDbContext
                .Appointments
                .FirstOrDefaultAsync(a => a.DayId == dayId && a.DoctorId == doctorId);
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
