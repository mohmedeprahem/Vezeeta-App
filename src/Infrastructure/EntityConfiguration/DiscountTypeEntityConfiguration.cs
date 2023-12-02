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
    internal class DiscountTypeEntityConfiguration : IEntityTypeConfiguration<DiscountType>
    {
        private IEnumService _enumService;

        public DiscountTypeEntityConfiguration(IEnumService enumService)
        {
            _enumService = enumService;
        }

        public void Configure(EntityTypeBuilder<DiscountType> builder)
        {
            builder.Property(dt => dt.Name).HasConversion<string>().HasMaxLength(20);

            // Create a constraint for the Gender enum
            string NameConstraint = _enumService.GetEnumCheckConstraint<DiscountTypeEnum>("Name");

            builder.ToTable(
                table => table.HasCheckConstraint("CK_DiscountTypes_Name", NameConstraint)
            );
        }
    }
}
