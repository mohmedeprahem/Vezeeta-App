using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
