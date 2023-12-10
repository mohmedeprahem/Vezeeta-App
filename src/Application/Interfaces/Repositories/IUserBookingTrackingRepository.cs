using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Repositories
{
    public interface IUserBookingTrackingRepository
    {
        public Task<IdentityResult> Create(UserBookingTracking userBookingTracking);
        public Task<IdentityResult> Update(UserBookingTracking userBookingTracking);
        public Task<UserBookingTracking> GetById(string id);
    }
}
