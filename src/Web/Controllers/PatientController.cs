using System.Collections.Generic;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientRepository;

        public PatientController(IPatientService patientRepository)
        {
            this._patientRepository = patientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients(
            [FromQuery] int page = 1,
            [FromQuery] int size = 1
        )
        {
            try
            {
                if (page < 0 || size < 0)
                {
                    return BadRequest();
                }

                // Get patients
                List<ApplicationUser> patientsInfo = await _patientRepository.GetPatients(
                    page,
                    size
                );

                if (patientsInfo == null)
                {
                    return NotFound();
                }

                // Format response
                var patientsResponse = patientsInfo
                    .Select(
                        patient =>
                            new
                            {
                                image = patient.Image,
                                fullName = $"{patient.FirstName} {patient.LastName}",
                                email = patient.Email,
                                gender = patient.Gender.ToString(),
                                dateOfBirth = patient.DateOfBirth.ToString("dd/MM/yyyy"),
                            }
                    )
                    .ToList();

                // Get total number of patients
                int totalPatientsCount = await _patientRepository.GetPatientsCount();

                int maxPages = (int)Math.Ceiling((double)totalPatientsCount / size);

                return Ok(
                    new
                    {
                        statusCode = 200,
                        totalPatientsCount,
                        maxPages,
                        currentPage = page,
                        itemsPerPage = size,
                        patients = patientsResponse
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
