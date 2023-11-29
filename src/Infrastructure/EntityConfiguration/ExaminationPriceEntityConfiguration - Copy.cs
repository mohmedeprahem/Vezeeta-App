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
    internal class UserBookingTrackingEntityConfiguration
        : IEntityTypeConfiguration<UserBookingTracking>
    {
        public void Configure(EntityTypeBuilder<UserBookingTracking> builder)
        {
            builder.HasKey(bt => bt.PatientId);

            builder
                .HasOne(bt => bt.Patient)
                .WithOne(u => u.AprovedBookingCount)
                .HasForeignKey<UserBookingTracking>(ep => ep.PatientId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
