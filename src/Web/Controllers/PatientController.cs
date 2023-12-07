using System.Collections.Generic;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            this._patientService = patientService;
        }

        [HttpGet]
        [Authorize(policy: "AdminOnly")]
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
                List<ApplicationUser> patientsInfo = await _patientService.GetPatients(
                    page,
                    size,
                    search
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
                                id = patient.Id,
                                image = patient.Image,
                                fullName = patient.FullName,
                                email = patient.Email,
                                phoneNumber = patient.PhoneNumber,
                                gender = patient.Gender.ToString(),
                                dateOfBirth = patient.DateOfBirth.ToString("dd/MM/yyyy"),
                            }
                    )
                    .ToList();

                // Get total number of patients
                int totalPatientsCount = await _patientService.GetPatientsCount();

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

        [HttpGet("count")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> GetPatientsCount([FromQuery] string lastDate = "")
        {
            try
            {
                int numberOfPatients = await _patientService.GetPatientsCount(lastDate);
                return Ok(
                    new
                    {
                        numberOfPatients,
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

        [HttpGet("{id}")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> GetPatientById([FromRoute] string id)
        {
            try
            {
                ApplicationUser patientsInfo = await _patientService.GetPatientById(id);

                if (patientsInfo == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        succes = true,
                        statusCode = 200,
                        patient = new
                        {
                            image = patientsInfo.Image,
                            fullName = patientsInfo.FullName,
                            email = patientsInfo.Email,
                            phoneNumber = patientsInfo.PhoneNumber,
                            gender = patientsInfo.Gender.ToString(),
                            dateOfBirth = patientsInfo.DateOfBirth.ToString("dd/MM/yyyy")
                        },
                        requests = Array.Empty<Booking>()
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
