using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
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
        public async Task<IActionResult> CreateAppointmentDayAsync(
            [FromForm] CreateAppointmentDto createAppointmentDto
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

                await _appointmentService.CreateAppointmentAsync(
                    createAppointmentDto.Appointments,
                    userId,
                    createAppointmentDto.Price
                );
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
