﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Application.Services;
using Core.enums;
using Core.Models;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfiguration
{
    internal class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        private IEnumService _enumService;

        public ApplicationUserEntityConfiguration(IEnumService enumService)
        {
            _enumService = enumService;
        }

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.Gender).HasConversion<string>().HasMaxLength(20);

            // Create a constraint for the Gender enum
            string genderConstraint = _enumService.GetEnumCheckConstraint<GendersEnum>("Gender");

            builder.ToTable(
                table => table.HasCheckConstraint("CK_AspNetUsers_Gender", genderConstraint)
            );

            builder.HasIndex(u => u.Email).IsUnique();

            builder
                .Property(u => u.FullName)
                .HasComputedColumnSql("[FirstName] + ' ' + [LastName]")
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(e => e.CreatedAt).HasDefaultValueSql("getutcdate()");
        }
    }
}
