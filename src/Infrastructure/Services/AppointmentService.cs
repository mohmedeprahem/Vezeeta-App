using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.enums;
using Core.Models;
using Infrastructure.Helpers.GeneralFunctions;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HelperFunctions _helperFunctions;

        public AppointmentService(
            IDoctorRepository doctorRepository,
            IUnitOfWork unitOfWork,
            HelperFunctions helperFunctions
        )
        {
            this._doctorRepository = doctorRepository;
            this._unitOfWork = unitOfWork;
            this._helperFunctions = helperFunctions;
        }

        public async Task<IdentityResult> CreateAppointmentAsync(
            List<AppointmentDto> createAppointmentDto,
            string doctorId,
            int price
        )
        {
            // Check if doctor exists
            ApplicationUser doctor = await _doctorRepository.GetDoctorById(doctorId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }
            await _unitOfWork.BeginTransactionAsync();

            // Update price
            await _unitOfWork
                .ExaminationPriceRepository
                .UpdateExaminationPrices(
                    new ExaminationPrice { DoctorId = doctorId, price = price }
                );
            {
                try
                {
                    // Create appointment
                    foreach (AppointmentDto appointmentDayDto in createAppointmentDto)
                    {
                        // Check if day valid
                        if (Enum.TryParse(appointmentDayDto.Day, out DaysEnum day))
                        {
                            // Check if the appointment day grater than today
                            DateOnly nextWeekday = _helperFunctions.GetNextWeekday(day.ToString());

                            DateOnly currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

                            if (nextWeekday <= currentDate)
                            {
                                throw new Exception("Invalid day");
                            }

                            // Check if the appointment day already exists
                            bool IsDayAdded = await _unitOfWork
                                .AppointmentRepository
                                .IsDayAddedBefore((int)day, doctorId);
                            if (IsDayAdded)
                            {
                                throw new Exception("this appointment already exist");
                            }

                            // Create appointment day
                            Appointment appointmentDay = await _unitOfWork
                                .AppointmentRepository
                                .CreateAppointmentDayAsync(
                                    new Appointment { DayId = (int)day, DoctorId = doctorId }
                                );
                            await _unitOfWork.SaveChangesAsync();

                            // Check if there duplication in appointment times
                            bool hasDuplicates =
                                appointmentDayDto.Times.Count()
                                != appointmentDayDto.Times.Distinct().Count();
                            if (hasDuplicates)
                            {
                                throw new Exception("Invalid time");
                            }
                            // Create appointment times
                            foreach (string time in appointmentDayDto.Times)
                            {
                                // Check if time valid
                                TimeOnly parsedTime = TimeOnly.ParseExact(
                                    time,
                                    "h:mm tt",
                                    CultureInfo.InvariantCulture
                                );
                                await _unitOfWork
                                    .AppointmentRepository
                                    .CreateAppointmentTimeAsync(appointmentDay.Id, parsedTime);
                            }
                        }
                        else
                        {
                            throw new Exception("Invalid day");
                        }
                    }
                    await _unitOfWork.CommitAsync();

                    return IdentityResult.Success;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync();
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "Create New appointment failed",
                            Description = ex.Message
                        }
                    );
                }
            }
        }

        public async Task<IdentityResult> UpdateAppointmentTimeAsync(
            string doctorId,
            UpdateAppointmentTimeDto updateAppointmentTimeDto,
            int AppointmentTimeId
        )
        {
            AppointmentTime appointmentTime = await _unitOfWork
                .AppointmentRepository
                .GetAppointmentTimeById(AppointmentTimeId, ["Appointment"]);

            if (appointmentTime == null)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "NotFound",
                        Description = "Appointment time not found"
                    }
                );
            }

            if (appointmentTime.Appointment.DoctorId != doctorId || appointmentTime.IsBooked)
            {
                return IdentityResult.Failed(
                    new IdentityError { Code = "NotAuthorized", Description = "Not authorized" }
                );
            }

            // Check if time valid
            TimeOnly parsedTime = TimeOnly.ParseExact(
                updateAppointmentTimeDto.Time,
                "h:mm tt",
                CultureInfo.InvariantCulture
            );

            // Update appointment time
            IdentityResult identityResult = await _unitOfWork
                .AppointmentRepository
                .UpdateAppointmentTimeAsync(AppointmentTimeId, parsedTime);

            await _unitOfWork.SaveChangesAsync();
            return identityResult;
        }

        public async Task<IdentityResult> DeleteAppointmentTimeAsync(
            string doctorId,
            int AppointmentTimeId
        )
        {
            AppointmentTime appointmentTime = await _unitOfWork
                .AppointmentRepository
                .GetAppointmentTimeById(AppointmentTimeId, ["Appointment"]);

            if (appointmentTime == null)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "NotFound",
                        Description = "Appointment time not found"
                    }
                );
            }

            if (appointmentTime.Appointment.DoctorId != doctorId || appointmentTime.IsBooked)
            {
                return IdentityResult.Failed(
                    new IdentityError { Code = "NotAuthorized", Description = "Not authorized" }
                );
            }

            // Delete appointment time
            await _unitOfWork.AppointmentRepository.DeleteAppointmentTimeAsync(AppointmentTimeId);
            await _unitOfWork.SaveChangesAsync();
            return IdentityResult.Success;
        }
    }
}
