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
                            DateTime nextWeekday = _helperFunctions.GetNextWeekday(day.ToString());

                            if (nextWeekday <= DateTime.UtcNow)
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

                            /*// Create appointment times
                            foreach (string time in appointmentDayDto.Times)
                            {
                                await _unitOfWork
                                    .AppointmentRepository
                                    .CreateAppointmentTimeAsync(appointmentDay.Id, time);
                            }*/
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
                        new IdentityError { Code = "TransactionFailed", Description = ex.Message }
                    );
                }
            }
        }
    }
}
