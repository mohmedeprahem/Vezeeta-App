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
using Microsoft.AspNetCore.Identity;
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

            DateOnly nextWeekday = _helperFunctions.GetNextWeekday(dayEnum.ToString());

            return await _appDbContext
                .Appointments
                .AnyAsync(a => a.DayId == dayId && a.DoctorId == doctorId && a.Date == nextWeekday);
        }

        public async Task<AppointmentTime> GetAppointmentTimeById(int id, string[] includes = null)
        {
            IQueryable<AppointmentTime> query = _appDbContext.Set<AppointmentTime>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(at => at.Id == id);
        }

        public async Task<IdentityResult> ChangeAppointmentTimeStatus(int id)
        {
            try
            {
                AppointmentTime? appointmentTime = await _appDbContext
                    .AppointmentTimes
                    .FindAsync(id);
                if (appointmentTime == null)
                {
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "AppointmentTime",
                            Description = "AppointmentTime not found"
                        }
                    );
                }
                else
                {
                    appointmentTime.IsBooked = !appointmentTime.IsBooked;
                    return IdentityResult.Success;
                }
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> UpdateAppointmentTimeAsync(
            int appointmentTimeId,
            TimeOnly time
        )
        {
            Time timeEntity = await GetOrCreateTimeEntityAsync(time);

            AppointmentTime? appointmentTime = await _appDbContext
                .AppointmentTimes
                .FindAsync(appointmentTimeId);

            if (appointmentTime == null)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "AppointmentTimeNotFound",
                        Description = "AppointmentTime not found."
                    }
                );
            }

            appointmentTime.TimeId = timeEntity.Id;
            return IdentityResult.Success;
        }

        private async Task<Time> GetOrCreateTimeEntityAsync(TimeOnly time)
        {
            // Check if the Time entity with the specified TimeValue already exists
            Time? timeEntity = await _appDbContext
                .Times
                .FirstOrDefaultAsync(t => t.TimeValue == time);

            if (timeEntity == null)
            {
                // If it doesn't exist, create a new Time entity
                timeEntity = new Time { TimeValue = time };

                // Add the new Time entity to the database
                await _appDbContext.Times.AddAsync(timeEntity);
                await _appDbContext.SaveChangesAsync();
            }

            return timeEntity;
        }

        public async Task<IdentityResult> DeleteAppointmentTimeAsync(int id)
        {
            var appointmentTime = await _appDbContext.AppointmentTimes.FindAsync(id);

            if (appointmentTime == null)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "NotFound",
                        Description = "AppointmentTime not found."
                    }
                );
            }

            _appDbContext.AppointmentTimes.Remove(appointmentTime);

            return IdentityResult.Success;
        }
    }
}
