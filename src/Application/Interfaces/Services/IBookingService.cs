using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Services
{
    public interface IBookingService
    {
        public Task<IdentityResult> CreateBookingAsync(
            int appointmentTimeId,
            string? discountCode,
            string patientId
        );

        public Task<IdentityResult> ConfirmBookingAsync(int bookingId, string doctorId);
        public Task<IdentityResult> CancelBookingAsync(int bookingId, string patientId);
        public Task<NumOfRequestsDto> GetBookingCountsAsync(string lastDate = "");
        public Task<List<TopSpecializationDto>> GetTopSpecializationByBooking();
        public Task<List<Booking>> GetBookingsByPatientIdAsync(
            string patientId,
            string[] includes = null
        );
    }
}
