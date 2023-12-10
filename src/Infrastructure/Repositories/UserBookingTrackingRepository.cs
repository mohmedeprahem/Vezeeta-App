using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Core.Models;
using Infrastructure.DataBase.Context;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class UserBookingTrackingRepository : IUserBookingTrackingRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserBookingTrackingRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        // Create a new UserBookingTracking
        public async Task<IdentityResult> Create(UserBookingTracking userBookingTracking)
        {
            await _appDbContext.UserBookingTracking.AddAsync(userBookingTracking);
            return IdentityResult.Success;
        }

        // Update a UserBookingTracking
        public async Task<IdentityResult> Update(UserBookingTracking userBookingTracking)
        {
            _appDbContext.UserBookingTracking.Update(userBookingTracking);
            return IdentityResult.Success;
        }

        // Get a UserBookingTracking
        public async Task<UserBookingTracking> GetById(string id)
        {
            return await _appDbContext.UserBookingTracking.FindAsync(id);
        }
    }
}
