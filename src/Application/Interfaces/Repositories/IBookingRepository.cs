using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        public Task<IdentityResult> CreateBookingAsync(Booking booking);
        public Task<Booking> GetBookingByIdAsync(int id, string[] includes = null);
        public Task<NumOfRequestsDto> GetBookingCountsAsync(string lastDate = "");
        public Task<List<TopSpecializationDto>> GetTopSpecializationByBooking();
        public Task<List<Booking>> GetBookingsByPatientIdAsync(
            string patientId,
            string[] includes = null
        );
    }
}
