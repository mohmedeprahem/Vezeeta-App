using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Repositories
{
    public interface IDiscountRepository
    {
        public Task<IdentityResult> CreateDiscountAsync(Discount discount);
        public Task<Discount> GetDiscountByCodeAsync(string name);
    }
}
