using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Core.enums;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiscountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IdentityResult> CreateDiscountAsync(CreateDiscountDto discountDto)
        {
            try
            {
                Discount discount = _mapper.Map<Discount>(discountDto);
                Discount IsDiscountExist = await _unitOfWork
                    .DiscountRepository
                    .GetDiscountByCodeAsync(discount.DiscountCode);
                if (IsDiscountExist != null)
                {
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "DuplicateCode",
                            Description = "Discount Already Exist"
                        }
                    );
                }

                IdentityResult IsDiscountCreated = await _unitOfWork
                    .DiscountRepository
                    .CreateDiscountAsync(discount);
                await _unitOfWork.SaveChangesAsync();
                return IsDiscountCreated;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "Create New appointment failed",
                        Description = ex.ToString()
                    }
                );
            }
        }
    }
}
