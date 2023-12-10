using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Repositories;
using Core.Models;
using Infrastructure.DataBase.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext _appDbContext;

        public DiscountRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<IdentityResult> CreateDiscountAsync(Discount discount)
        {
            try
            {
                // Check if discount already exists
                await _appDbContext.Discounts.AddAsync(discount);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(
                    new IdentityError { Code = "DiscountCreationFailed", Description = ex.Message }
                );
            }
        }

        public async Task<Discount> GetDiscountByCodeAsync(string code)
        {
            Discount? discount = await _appDbContext
                .Discounts
                .FirstOrDefaultAsync(d => d.DiscountCode == code);

            return discount;
        }

        public async Task<Discount> GetDiscountById(int id, string[] includes = null)
        {
            IQueryable<Discount> query = _appDbContext.Set<Discount>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstAsync(d => d.Id == id);
        }

        public async Task<IdentityResult> UpdateDiscount(Discount updatedDiscount, int id)
        {
            var existingDiscount = await _appDbContext.Discounts.FindAsync(id);

            if (existingDiscount == null)
            {
                return IdentityResult.Failed(
                    new IdentityError { Code = "NotFound", Description = "Discount not found" }
                );
            }

            existingDiscount.DiscountCode = updatedDiscount.DiscountCode;
            existingDiscount.DiscountTypeId = updatedDiscount.DiscountTypeId;
            existingDiscount.IsActivated = updatedDiscount.IsActivated;
            existingDiscount.DiscountValue = updatedDiscount.DiscountValue;

            return IdentityResult.Success;
        }
    }
}
