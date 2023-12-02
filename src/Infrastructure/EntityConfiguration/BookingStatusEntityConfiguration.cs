using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.enums;
using Core.Models;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    internal class BookingStatusEntityConfiguration : IEntityTypeConfiguration<BookingStatus>
    {
        private IEnumService _enumService;

        public BookingStatusEntityConfiguration(IEnumService enumService)
        {
            _enumService = enumService;
        }

        public void Configure(EntityTypeBuilder<BookingStatus> builder)
        {
            builder.Property(bs => bs.Name).HasConversion<string>().HasMaxLength(20);

            // Create a constraint for the Gender enum
            string BookingStatusConstraint = _enumService.GetEnumCheckConstraint<BookingStatusEnum>(
                "Name"
            );

            builder.ToTable(
                table => table.HasCheckConstraint("CK_AspNetUsers_Gender", BookingStatusConstraint)
            );
        }
    }
}
