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
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();

            // Seed roles
            var roleManager = serviceScope
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();
            foreach (string role in Enum.GetNames(typeof(RolesEnum)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }

            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            // Seed Admin user
            var userManager = serviceScope
                .ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            if (!userManager.Users.Any(u => u.Email == "admin@localhost.com"))
            {
                // Create a new user
                ApplicationUser adminUser = new ApplicationUser
                {
                    FirstName = "Mohamed",
                    LastName = "Ibrahem",
                    UserName = "admin",
                    Email = "admin@localhost.com",
                    EmailConfirmed = true,
                    PhoneNumber = "+201234567891",
                    PhoneNumberConfirmed = true
                };

                IdentityResult createResult = await userManager.CreateAsync(
                    adminUser,
                    "@Mohmed123"
                );

                if (createResult.Succeeded)
                {
                    // If user creation is successful, assign a role to the user (assuming you have a role called "Admin")
                    await userManager.AddToRoleAsync(adminUser, RolesEnum.Admin.ToString());
                }
            }

            // Seed days
            if (!context.Days.Any())
            {
                foreach (var day in Enum.GetValues(typeof(DaysEnum)).Cast<DaysEnum>())
                {
                    await context.Days.AddAsync(new Day { Name = day });
                }
            }

            // Seed BookingStatus
            if (!context.BookingStatuses.Any())
            {
                foreach (
                    var status in Enum.GetValues(typeof(BookingStatusEnum))
                        .Cast<BookingStatusEnum>()
                )
                {
                    await context.BookingStatuses.AddAsync(new BookingStatus { Name = status });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
