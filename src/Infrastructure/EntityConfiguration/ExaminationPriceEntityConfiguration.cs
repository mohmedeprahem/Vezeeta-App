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
    internal class ExaminationPriceEntityConfiguration : IEntityTypeConfiguration<ExaminationPrice>
    {
        public void Configure(EntityTypeBuilder<ExaminationPrice> builder)
        {
            builder.HasKey(ep => ep.DoctorId);

            builder.HasOne(ep => ep.Doctor).WithOne(d => d.Price).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
