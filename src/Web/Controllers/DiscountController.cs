using Application.Dtos;
using Application.Interfaces.Services;
using Core.enums;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/discount")]
    [ApiController]
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            this._discountService = discountService;
        }

        [HttpPost]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> CreateDiscount(
            [FromBody] CreateDiscountDto createDiscountDto
        )
        {
            try
            {
                // Check if discount type exists
                if (!Enum.IsDefined(typeof(DiscountTypeEnum), createDiscountDto.DiscountTypeId))
                {
                    ModelState.AddModelError("DiscountTypeId", "Invalid discount type");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                IdentityResult identityResult = await _discountService.CreateDiscountAsync(
                    createDiscountDto
                );

                if (!identityResult.Succeeded)
                {
                    if (identityResult.Errors.Any(error => error.Code == "DuplicateCode"))
                    {
                        ModelState.AddModelError("Code", "Discount name already exists.");
                        return StatusCode(
                            403,
                            new
                            {
                                success = false,
                                statusCode = 403,
                                messgae = ModelState,
                            }
                        );
                    }

                    return BadRequest(identityResult);
                }

                return StatusCode(
                    201,
                    new
                    {
                        success = true,
                        statusCode = 201,
                        messgae = "Discount created successfully.",
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
