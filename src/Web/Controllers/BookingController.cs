using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Models;
using Infrastructure.Services;
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
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{bookingId}/confirm")]
        [Authorize(policy: "DoctorOnly")]
        public async Task<IActionResult> ConfirmBooking(int bookingId)
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

                IdentityResult bookingResult = await _bookingService.ConfirmBookingAsync(
                    bookingId,
                    userId
                );

                if (!bookingResult.Succeeded)
                {
                    if (!bookingResult.Succeeded)
                    {
                        if (bookingResult.Errors.Any(error => error.Code == "NotFound"))
                        {
                            return NotFound();
                        }
                        else if (bookingResult.Errors.Any(error => error.Code == "NotAuthorized"))
                        {
                            return Forbid();
                        }
                        else
                        {
                            throw new Exception("Failed to update appointment time");
                        }
                    }
                }
                return Ok(
                    new
                    {
                        succes = true,
                        statusCode = 200,
                        message = "Booking confirmed successfully"
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("{bookingId}/cancel")]
        [Authorize(policy: "PatientOnly")]
        public async Task<IActionResult> CancelBooking(int bookingId)
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

                IdentityResult bookingResult = await _bookingService.CancelBookingAsync(
                    bookingId,
                    userId
                );

                if (!bookingResult.Succeeded)
                {
                    if (!bookingResult.Succeeded)
                    {
                        if (bookingResult.Errors.Any(error => error.Code == "NotFound"))
                        {
                            return NotFound();
                        }
                        else if (bookingResult.Errors.Any(error => error.Code == "NotAuthorized"))
                        {
                            return Forbid();
                        }
                        else
                        {
                            throw new Exception("Failed to update appointment time");
                        }
                    }
                }
                return Ok(
                    new
                    {
                        succes = true,
                        statusCode = 200,
                        message = "Booking cancelled successfully"
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("count")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> GetBookingsCount([FromQuery] string lastDate = "")
        {
            try
            {
                NumOfRequestsDto numberOfDoctors = await _bookingService.GetBookingCountsAsync(
                    lastDate
                );

                return Ok(
                    new
                    {
                        succes = true,
                        statusCode = 200,
                        numberOfBookings = numberOfDoctors
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
