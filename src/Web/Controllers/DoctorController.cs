using Application.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/Doctors")]
    [ApiController]
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            this._doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients(
            [FromQuery] int page = 1,
            [FromQuery] int size = 1,
            [FromQuery] string search = ""
        )
        {
            try
            {
                if (page < 0 || size < 0)
                {
                    return BadRequest();
                }

                // Get patients
                List<ApplicationUser> doctorsInfo = await _doctorService.GetDoctors(
                    page,
                    size,
                    search
                );

                if (doctorsInfo == null)
                {
                    return NotFound();
                }

                // Format response
                var doctorsResponse = doctorsInfo
                    .Select(
                        doctor =>
                            new
                            {
                                image = doctor.Image,
                                fullName = doctor.FullName,
                                email = doctor.Email,
                                gender = doctor.Gender.ToString(),
                                dateOfBirth = doctor.DateOfBirth.ToString("dd/MM/yyyy"),
                                specialize = doctor.Specialization != null
                                    ? doctor.Specialization.Title
                                    : null
                            }
                    )
                    .ToList();

                // Get total number of patients
                int totalDoctorsCount = await _doctorService.GetDoctorsCount();

                int maxPages = (int)Math.Ceiling((double)totalDoctorsCount / size);

                return Ok(
                    new
                    {
                        statusCode = 200,
                        totalDoctorsCount,
                        maxPages,
                        currentPage = page,
                        itemsPerPage = size,
                        doctors = doctorsResponse
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
