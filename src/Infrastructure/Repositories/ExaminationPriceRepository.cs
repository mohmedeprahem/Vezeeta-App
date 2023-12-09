using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Core.Models;
using Infrastructure.DataBase.Context;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories
{
    public class ExaminationPriceRepository : IExaminationPriceRepository
    {
        private readonly AppDbContext _appDbContext;

        public ExaminationPriceRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<IdentityResult> CreateExaminationPrices(ExaminationPrice examinationPrice)
        {
            try
            {
                await _appDbContext.ExaminationPrices.AddAsync(examinationPrice);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = $"An error occurred: {ex.Message}" }
                );
            }
        }

        public async Task<ExaminationPrice> GetExaminationPrices(string id)
        {
            return await _appDbContext.ExaminationPrices.FindAsync(id);
        }

        public async Task<IdentityResult> UpdateExaminationPrices(
            ExaminationPrice updatedExaminationPrice
        )
        {
            try
            {
                var existingExaminationPrice = await _appDbContext
                    .ExaminationPrices
                    .FindAsync(updatedExaminationPrice.DoctorId);

                if (existingExaminationPrice == null)
                {
                    return IdentityResult.Failed(
                        new IdentityError { Description = "ExaminationPrice not found." }
                    );
                }
                existingExaminationPrice.price = updatedExaminationPrice.price;

                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = $"An error occurred: {ex.Message}" }
                );
            }
        }
    }
}
