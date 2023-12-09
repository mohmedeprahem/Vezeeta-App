using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            this._appointmentService = appointmentService;
        }

        [HttpPost]
        [Authorize(policy: "DoctorOnly")]
        public async Task<IActionResult> CreateAppointmentDayAsync(
            [FromBody] CreateAppointmentDto createAppointmentDto
        )
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                // Access claims from the current user's ClaimsPrincipal
                ClaimsPrincipal user = HttpContext.User;

                // Example: Get the value of a specific claim
                string? userId = user.FindFirst("Id")?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }

                IdentityResult appointmentResult = await _appointmentService.CreateAppointmentAsync(
                    createAppointmentDto.Appointments,
                    userId,
                    createAppointmentDto.Price
                );

                if (!appointmentResult.Succeeded)
                {
                    return BadRequest(appointmentResult);
                }
                return StatusCode(
                    201,
                    new
                    {
                        success = true,
                        statusCode = 201,
                        messgae = "Appointment created successfully.",
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
