﻿using Application.Dtos;
using Application.Interfaces.Services;
using Core.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> GetDoctors(
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
                                id = doctor.Id,
                                image = doctor.Image,
                                fullName = doctor.FullName,
                                email = doctor.Email,
                                gender = doctor.Gender.ToString(),
                                phoneNumber = doctor.PhoneNumber,
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

        [HttpGet("{Id}")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> GetDoctorById([FromRoute] string Id)
        {
            try
            {
                ApplicationUser doctorInfo = await _doctorService.GetDoctorById(Id);

                if (doctorInfo == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        succes = true,
                        statusCode = 200,
                        Doctor = new
                        {
                            image = doctorInfo.Image,
                            fullName = doctorInfo.FullName,
                            email = doctorInfo.Email,
                            gender = doctorInfo.Gender.ToString(),
                            phoneNumber = doctorInfo.PhoneNumber,
                            dateOfBirth = doctorInfo.DateOfBirth.ToString("dd/MM/yyyy"),
                            specialize = doctorInfo.Specialization != null
                        },
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
}
