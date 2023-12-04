using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Core.enums;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    internal class DayEntityConfiguration : IEntityTypeConfiguration<Day>
    {
        private IEnumService _enumService;

        public DayEntityConfiguration(IEnumService enumService)
        {
            _enumService = enumService;
        }

        public void Configure(EntityTypeBuilder<Day> builder)
        {
            builder.Property(d => d.Name).HasConversion<string>().HasMaxLength(20);

            // Create a constraint for the Gender enum
            string DayConstraint = _enumService.GetEnumCheckConstraint<DaysEnum>("Name");

            builder.ToTable(table => table.HasCheckConstraint("CK_Days_Name", DayConstraint));
        }
    }
}
