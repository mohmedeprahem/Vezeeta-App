﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Infrastructure.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<ExaminationPrice> ExaminationPrices { get; set; }
        public DbSet<UserBookingTracking> UserBookingTracking { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentDay> AppointmentDays { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration<ExaminationPrice>(new ExaminationPriceEntityConfiguration());
            builder.ApplyConfiguration<UserBookingTracking>(
                new UserBookingTrackingEntityConfiguration()
            );
        }
    }
}
