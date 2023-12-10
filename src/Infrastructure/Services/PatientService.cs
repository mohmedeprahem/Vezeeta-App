using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
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

        public async Task<List<ApplicationUser>> GetPatients(int page, int size, string search)
        {
            return await _patientRepository.GetUsersByRole(
                RolesEnum.Patient.ToString(),
                page = 1,
                size = 8,
                search = ""
            );
        }

        public async Task<int> GetPatientsCount(string lastDate = "")
        {
            return await _patientRepository.GetUsersCountByRole(
                RolesEnum.Patient.ToString(),
                lastDate
            );
        }

        public async Task<PatientAllInfoDto> GetAllInfoPatientById(string id)
        {
            ApplicationUser user = await _patientRepository.GetUserById(
                id,

                [
                    "PatientBookings",
                    "PatientBookings.Specialization",
                    "PatientBookings.Discount",
                    "PatientBookings.BookingStatus",
                    "PatientBookings.AppointmentTime",
                    "PatientBookings.AppointmentTime.Time",
                    "PatientBookings.AppointmentTime.Appointment",
                    "PatientBookings.AppointmentTime.Appointment.Day",
                    "PatientBookings.AppointmentTime.Appointment",
                    "PatientBookings.AppointmentTime.Appointment.Doctor"
                ]
            );

            PatientAllInfoDto userDto = new PatientAllInfoDto
            {
                Image = user.Image,
                FullName = user.FullName,
                Email = user.Email,
                Gender = user.Email,
                DateOfBirth = user.DateOfBirth.ToString("dd/MM/yyyy"),
                Requests = user.PatientBookings
                    .Select(
                        booking =>
                            new BookingDto
                            {
                                Image = booking.AppointmentTime.Appointment.Doctor.Image,
                                DoctorName = booking.AppointmentTime.Appointment.Doctor.FullName,
                                Specialize = booking.Specialization.Title,
                                Day = booking.AppointmentTime.Appointment.Day.Name.ToString(),
                                Time = booking.AppointmentTime.Time.TimeValue.ToString("h:mm tt"), // Assuming Time is a DateTime property
                                Price = booking.Price,
                                DiscountCode = booking.Discount?.DiscountCode,
                                FinalPrice = booking.FinalPrice,
                                Status = booking.BookingStatus.Name.ToString() // Assuming BookingStatus is a navigation property
                            }
                    )
                    .ToList()
            };

            return userDto;
        }
    }
}
