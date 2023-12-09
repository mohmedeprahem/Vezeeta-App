﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Services
{
    public interface IDiscountService
    {
        public Task<IdentityResult> CreateDiscountAsync(CreateDiscountDto discountDto);
    }
}