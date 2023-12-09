using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        public Task<IdentityResult> CreateBookingAsync(Booking booking);
    }
}
