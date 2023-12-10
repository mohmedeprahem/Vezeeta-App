using Application.Dtos;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            this._doctorService = doctorService;
        }

        [HttpGet("count")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> GetDoctorsCount([FromQuery] string lastDate = "")
        {
            try
            {
                int numberOfDoctors = await _doctorService.GetDoctorsCount(lastDate);
                return Ok(
                    new
                    {
                        numberOfDoctors,
                        succes = true,
                        statusCode = 200
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{doctorId}")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> UpdateDoctor(
            [FromRoute] string doctorId,
            [FromForm] UpdateDoctorDto doctorDto
        )
        {
            try
            {
                // Validate image
                if (doctorDto.Image != null)
                {
                    long fileSize = doctorDto.Image.Length;

                    if (
                        doctorDto.Image.ContentType != "image/jpeg"
                        && doctorDto.Image.ContentType != "image/png"
                    )
                    {
                        ModelState.AddModelError(
                            "Image",
                            "Invalid image format. Only JPEG and PNG are allowed."
                        );
                    }
                    else if (fileSize > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError(
                            "Image",
                            "Invalid image size. The maximum allowed size is 5 MB."
                        );
                    }
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                IdentityResult statusDoctorUpdate = await _doctorService.UpdateDoctor(
                    doctorId,
                    doctorDto
                );

                if (!statusDoctorUpdate.Succeeded)
                {
                    return BadRequest(statusDoctorUpdate.Errors);
                }

                return Ok(
                    new
                    {
                        succes = true,
                        statusCode = 200,
                        message = "Doctor updated successfully"
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
}
