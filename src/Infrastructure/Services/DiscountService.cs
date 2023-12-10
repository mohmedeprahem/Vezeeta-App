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
                        Description = "Creation appointment failed"
                    }
                );
            }
        }

        public async Task<IdentityResult> UpdateDiscountAsync(
            int discountId,
            UpdateDiscountDto discountDto
        )
        {
            Discount updatedDiscount = _mapper.Map<Discount>(discountDto);
            Discount discount = await _unitOfWork
                .DiscountRepository
                .GetDiscountById(discountId, ["Bookings"]);

            // Check if discount exists
            if (discount == null)
            {
                return IdentityResult.Failed(
                    new IdentityError { Code = "NotFound", Description = "Discount Not Found" }
                );
            }

            // Check if discountValeue is valid
            if (updatedDiscount.DiscountTypeId == (int)DiscountTypeEnum.Percentage)
                if (updatedDiscount.DiscountValue > 100)
                {
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "InvalidRequest",
                            Description = "Discount Value Invalid"
                        }
                    );
                }

            // Check if discount has bookings
            if (discount.Bookings.Count > 0)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "BookingsExist",
                        Description = "Discount Bookings Exist"
                    }
                );
            }

            // Update discount
            IdentityResult discountResult = await _unitOfWork
                .DiscountRepository
                .UpdateDiscount(updatedDiscount, discountId);

            await _unitOfWork.SaveChangesAsync();
            return discountResult;
        }

        public async Task<IdentityResult> DeleteDiscountAsync(int discountId)
        {
            // Retrieve the discount entity by ID
            var discount = await _unitOfWork
                .DiscountRepository
                .GetDiscountById(discountId, ["Bookings"]);

            if (discount == null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "NotFound" });
            }

            // Check if discount has bookings
            if (discount.Bookings.Count > 0)
            {
                return IdentityResult.Failed(
                    new IdentityError
                    {
                        Code = "BookingsExist",
                        Description = "Discount Bookings Exist"
                    }
                );
            }

            // Remove the discount entity from the repository
            IdentityResult discountResult = await _unitOfWork
                .DiscountRepository
                .DeleteDiscountAsync(discountId);

            await _unitOfWork.SaveChangesAsync();
            return discountResult;
        }

        public async Task<IdentityResult> DeactivateDiscountAsync(int discountId)
        {
            Discount discount = await _unitOfWork.DiscountRepository.GetDiscountById(discountId);

            if (discount == null)
            {
                return IdentityResult.Failed(new IdentityError { Code = "NotFound" });
            }

            IdentityResult discountResult = await _unitOfWork
                .DiscountRepository
                .DeactivateDiscountAsync(discountId);

            await _unitOfWork.SaveChangesAsync();
            return discountResult;
        }
    }
}
