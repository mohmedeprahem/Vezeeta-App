using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Azure;
using Core.enums;
using Core.Models;
using Infrastructure.DataBase.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PatientRepository : UserRepository, IPatientRepository
    {
        public PatientRepository(
            UserManager<ApplicationUser> userManager,
            AppDbContext appDbContext
        )
            : base(userManager, appDbContext) { }
    }
}
