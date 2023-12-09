using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            this._bookingService = bookingService;
        }

        [HttpPost]
        [Authorize(policy: "PatientOnly")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto createBookingDto)
        {
            try
            {
                // Access claims from the current user's ClaimsPrincipal
                ClaimsPrincipal user = HttpContext.User;

                // Get the value of a specific claim
                string? userId = user.FindFirst("Id")?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }

                IdentityResult bookingResult = await _bookingService.CreateBookingAsync(
                    createBookingDto.AppointmentTimeId,
                    createBookingDto.DiscountCode,
                    userId
                );
                if (!bookingResult.Succeeded)
                {
                    if (bookingResult.Errors.Any(error => error.Code == "DiscountCode"))
                        return BadRequest(bookingResult);
                    return Unauthorized();
                }

                return StatusCode(
                    201,
                    new
                    {
                        succes = true,
                        statusCode = 201,
                        message = "Booking created successfully"
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
