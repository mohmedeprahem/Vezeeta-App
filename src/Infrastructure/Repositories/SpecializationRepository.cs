using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Core.Models;
using Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        public readonly AppDbContext _appDbContext;

        public SpecializationRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<Specialization> GetSpecializationByTitle(string title)
        {
            return await _appDbContext.Specializations.FirstOrDefaultAsync(s => s.Title == title);
        }
    }
}
