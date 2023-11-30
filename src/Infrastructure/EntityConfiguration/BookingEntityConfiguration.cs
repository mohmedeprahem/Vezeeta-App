using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    internal class BookingEntityConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder
                .HasOne(b => b.Patient)
                .WithMany(u => u.PatientBookings)
                .HasForeignKey(b => b.PatientId);
            builder
                .HasOne(b => b.Doctor)
                .WithMany(u => u.DoctorBookings)
                .HasForeignKey(b => b.DoctorId);
        }
    }
}
