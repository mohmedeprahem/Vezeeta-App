using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Repositories
{
    public interface IExaminationPriceRepository
    {
        public Task<IdentityResult> CreateExaminationPrices(ExaminationPrice examinationPrice);
        public Task<ExaminationPrice> GetExaminationPrices(string id);
        public Task<IdentityResult> UpdateExaminationPrices(
            ExaminationPrice updatedExaminationPrice
        );
    }
}
