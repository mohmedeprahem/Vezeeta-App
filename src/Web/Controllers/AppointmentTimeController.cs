using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces.Services;
using Core.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/appointmentTimes")]
    [ApiController]
    public class AppointmentTimeController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentTimeController(IAppointmentService appointmentService)
        {
            this._appointmentService = appointmentService;
        }

        [HttpPut("{AppointmentTimeId}")]
        [Authorize(policy: "DoctorOnly")]
        public async Task<IActionResult> UpdateAppointmentTimeAsync(
            [FromRoute] int AppointmentTimeId,
            [FromBody] UpdateAppointmentTimeDto updateAppointmentTimeDto
        )
        {
            try
            {
                // Get doctor id
                ClaimsPrincipal user = HttpContext.User;
                string doctorId = user.FindFirst("Id")?.Value;
                if (doctorId == null)
                {
                    return Unauthorized();
                }

                // Update appointment
                IdentityResult result = await _appointmentService.UpdateAppointmentTimeAsync(
                    doctorId,
                    updateAppointmentTimeDto,
                    AppointmentTimeId
                );

                if (!result.Succeeded)
                {
                    if (result.Errors.Any(error => error.Code == "NotFound"))
                    {
                        return NotFound();
                    }
                    else if (result.Errors.Any(error => error.Code == "NotAuthorized"))
                    {
                        return Forbid();
                    }
                    else
                    {
                        throw new Exception("Failed to update appointment time");
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
