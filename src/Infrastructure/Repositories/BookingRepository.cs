using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces.Repositories;
using Core.enums;
using Core.Models;
using Infrastructure.DataBase.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _appDbContext;

        public BookingRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<IdentityResult> CreateBookingAsync(Booking booking)
        {
            try
            {
                await _appDbContext.Bookings.AddAsync(booking);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Code = ex.Message });
            }
        }

        public async Task<Booking> GetBookingByIdAsync(int id, string[] includes = null)
        {
            IQueryable<Booking> query = _appDbContext.Set<Booking>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(at => at.Id == id);
        }

        public async Task<NumOfRequestsDto> GetBookingCountsAsync(string lastDate = "")
        {
            NumOfRequestsDto numOfRequestsDto;

            DateTime lastWeekStartDate = lastDate switch
            {
                "day" => DateTime.UtcNow.AddDays(-1),
                "week" => DateTime.UtcNow.AddDays(-7),
                "month" => DateTime.UtcNow.AddDays(-30),
                "year" => DateTime.UtcNow.AddDays(-365),
                _ => DateTime.UtcNow.AddDays(-7)
            };

            IQueryable<Booking> query = _appDbContext.Bookings;

            if (!lastDate.IsNullOrEmpty())
            {
                query = query.Where(b => b.CreatedAt >= lastWeekStartDate);
            }

            var result = await query
                .GroupBy(b => b.BookingStatusId)
                .Select(group => new { StatusId = group.Key, Count = group.Count() })
                .ToListAsync();

            numOfRequestsDto = new NumOfRequestsDto
            {
                ConfirmedRequests =
                    result
                        .FirstOrDefault(r => r.StatusId == (int)BookingStatusEnum.Completed)
                        ?.Count ?? 0,
                PendingRequests =
                    result.FirstOrDefault(r => r.StatusId == (int)BookingStatusEnum.Binding)?.Count
                    ?? 0,
                CancelledRequests5 =
                    result
                        .FirstOrDefault(r => r.StatusId == (int)BookingStatusEnum.Cancelled)
                        ?.Count ?? 0
            };

            return numOfRequestsDto;
        }

        public async Task<List<TopSpecializationDto>> GetTopSpecializationByBooking()
        {
            List<TopSpecializationDto> topSpecializationDto;

            var result = await _appDbContext
                .Bookings
                .Include(b => b.Specialization)
                .GroupBy(b => b.SpecializationId)
                .Select(
                    group =>
                        new
                        {
                            SpecializationId = group.Key,
                            Count = group.Count(),
                            SpecializationName = group.FirstOrDefault().Specialization.Title
                        }
                )
                .Take(5)
                .ToListAsync();

            topSpecializationDto = result
                .Select(
                    item =>
                        new TopSpecializationDto
                        {
                            FullName = item.SpecializationName,
                            BookingCount = item.Count
                        }
                )
                .ToList();

            return topSpecializationDto;
        }

        public async Task<List<Booking>> GetBookingsByPatientIdAsync(
            string patientId,
            string[] includes = null
        )
        {
            IQueryable<Booking> query = _appDbContext.Set<Booking>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.Where(b => b.PatientId == patientId).ToListAsync();
        }
    }
}
