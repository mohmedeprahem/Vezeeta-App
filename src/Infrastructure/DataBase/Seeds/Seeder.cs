using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.enums;
using Core.Models;
using Infrastructure.DataBase.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataBase.Etension
{
    public static class Seeder
    {
        public static async void Seed(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            // Seed Admin user
            var userManager = serviceScope
                .ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            if (!userManager.Users.Any(u => u.Email == "admin@localhost.com"))
            {
                await userManager.CreateAsync(
                    new ApplicationUser
                    {
                        FirstName = "mohamed",
                        LastName = "ibrahem",
                        UserName = "admin",
                        Email = "admin@localhost.com",
                        EmailConfirmed = true,
                        PasswordHash = "admin@localhost"
                    }
                );
            }

            // Seed roles
            var roleManager = serviceScope
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await roleManager.RoleExistsAsync("Patient"))
                await roleManager.CreateAsync(new IdentityRole("Patient"));
            if (!await roleManager.RoleExistsAsync("Doctor"))
                await roleManager.CreateAsync(new IdentityRole("Doctor"));

            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            // Seed days
            if (!context.Days.Any())
            {
                foreach (var day in Enum.GetValues(typeof(DaysEnum)).Cast<DaysEnum>())
                {
                    await context.Days.AddAsync(new Day { Name = day });
                }
            }

            // Seed BookingStatus
            if (!context.bookingStatuses.Any())
            {
                foreach (
                    var status in Enum.GetValues(typeof(BookingStatusEnum))
                        .Cast<BookingStatusEnum>()
                )
                {
                    await context.bookingStatuses.AddAsync(new BookingStatus { Name = status });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
