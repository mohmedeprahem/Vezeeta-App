using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Models;
using Infrastructure.Helpers.GeneralFunctions;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly HelperFunctions _helperFunctions;

        public BookingService(
            IUnitOfWork unitOfWork,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            HelperFunctions helperFunctions
        )
        {
            this._unitOfWork = unitOfWork;
            this._patientRepository = patientRepository;
            this._doctorRepository = doctorRepository;
            this._helperFunctions = helperFunctions;
        }

        public async Task<IdentityResult> CreateBookingAsync(
            int appointmentTimeId,
            string? discountCode,
            string patientId
        )
        {
            try
            {
                int finalPrice = 0;
                Discount discount;
                // Check if patient exists
                ApplicationUser patient = await _patientRepository.GetUserById(patientId);
                if (patient == null)
                {
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "PatientNotFound",
                            Description = "Patient not found"
                        }
                    );
                }

                // Get appointment time
                AppointmentTime appointmentTime = await _unitOfWork
                    .AppointmentRepository
                    .GetAppointmentTimeById(appointmentTimeId, new string[] { "Appointment" });
                if (appointmentTime == null)
                {
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "Appointment",
                            Description = "Appointment not found"
                        }
                    );
                }

                if (appointmentTime.IsBooked)
                {
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "Appointment",
                            Description = "Appointment already booked"
                        }
                    );
                }
                // Get doctor
                ApplicationUser doctor = await _doctorRepository.GetUserById(
                    appointmentTime.Appointment.DoctorId,
                    new string[] { "Specialization", "ExaminationPrice" }
                );
                if (doctor == null)
                {
                    throw new Exception("Server Error");
                }
                finalPrice = doctor.ExaminationPrice.price;

                // Create booking
                Booking booking = new Booking()
                {
                    AppointmentTimeId = appointmentTimeId,
                    BookingStatusId = 1,
                    PatientId = patientId,
                    Date = appointmentTime.Appointment.Date,
                    Price = finalPrice,
                    SpecializationId = doctor.SpecializationId ?? 1
                };

                // Check if discount code is valid
                if (discountCode != null)
                {
                    discount = await _unitOfWork
                        .DiscountRepository
                        .GetDiscountByCodeAsync(discountCode);
                    if (discount == null)
                    {
                        return IdentityResult.Failed(
                            new IdentityError
                            {
                                Code = "DiscountCode",
                                Description = "Discount code not found"
                            }
                        );
                    }
                    else
                    {
                        if (!discount.IsActivated)
                        {
                            return IdentityResult.Failed(
                                new IdentityError
                                {
                                    Code = "DiscountCode",
                                    Description = "Discount code expired"
                                }
                            );
                        }
                        if (discount.DiscountTypeId == 1)
                        {
                            finalPrice = _helperFunctions.CalculatePercentageDiscount(
                                finalPrice,
                                discount.DiscountValue
                            );
                        }
                        else
                        {
                            finalPrice -= discount.DiscountValue;
                            if (finalPrice < 0)
                                finalPrice = 0;
                        }
                        discount.IsActivated = false;
                        booking.DiscountId = discount.Id;
                    }
                }

                booking.FinalPrice = finalPrice;

                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.BookingRepository.CreateBookingAsync(booking);

                // Update appointment time
                appointmentTime.IsBooked = true;

                await _unitOfWork.CommitAsync();

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception(ex.ToString());
            }
        }
    }
}
