using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.Services;
using Infrastructure.DataBase.Etension;
using Infrastructure.EntityConfiguration;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<ExaminationPrice> ExaminationPrices { get; set; }
        public DbSet<UserBookingTracking> UserBookingTracking { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<AppointmentTime> AppointmentTimes { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<DiscountType> DiscountTypes { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<BookingStatus> bookingStatuses { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ExaminationPriceEntityConfiguration());
            builder.ApplyConfiguration(new UserBookingTrackingEntityConfiguration());
            builder.ApplyConfiguration(new BookingEntityConfiguration());
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration(new EnumService()));
            builder.ApplyConfiguration(new BookingStatusEntityConfiguration(new EnumService()));
            builder.ApplyConfiguration(new DayEntityConfiguration(new EnumService()));
            builder.ApplyConfiguration(new DiscountTypeEntityConfiguration(new EnumService()));
        }
    }
}
