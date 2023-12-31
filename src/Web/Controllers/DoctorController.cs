﻿using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces.Services;
using Core.Models;
using Infrastructure.Helpers.GeneralFunctions;
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
        private readonly IBookingService _bookingService;
        private readonly HelperFunctions _helperFunctions;

        public DoctorController(
            IDoctorService doctorService,
            IBookingService bookingService,
            HelperFunctions helperFunctions
        )
        {
            this._doctorService = doctorService;
            this._bookingService = bookingService;
            this._helperFunctions = helperFunctions;
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

        [HttpGet]
        [Authorize(policy: "PatientOrAdminOnly")]
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
                // Access claims from the current user's ClaimsPrincipal
                ClaimsPrincipal user = HttpContext.User;

                // Get the value of a specific claim
                string? userRole = user.FindFirst(ClaimTypes.Role)?.Value;
                if (userRole == null)
                {
                    return Unauthorized();
                }
                List<ApplicationUser> doctorsInfo;
                // Get the value of a specific role
                if (userRole == "Admin")
                {
                    doctorsInfo = await _doctorService.GetDoctors(
                        page,
                        size,
                        search,
                        ["Specialization"]
                    );
                }
                else
                {
                    doctorsInfo = await _doctorService.GetDoctors(
                        page,
                        size,
                        search,

                        [
                            "Specialization",
                            "ExaminationPrice",
                            "Appointments",
                            "Appointments.Day",
                            "Appointments.Times",
                            "Appointments.Times.Time",
                        ]
                    );
                }
                // Get patients


                if (doctorsInfo == null)
                {
                    return NotFound();
                }

                // Get total number of patients
                int totalDoctorsCount = await _doctorService.GetDoctorsCountByString(search);

                int maxPages = (int)Math.Ceiling((double)totalDoctorsCount / size);

                if (userRole == "Admin")
                {
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
                else
                {
                    var doctorsResponse = doctorsInfo
                        .Select(
                            doctor =>
                                new
                                {
                                    Image = doctor.Image,
                                    FullName = doctor.FullName,
                                    Email = doctor.Email,
                                    Phone = doctor.PhoneNumber,
                                    Price = doctor.ExaminationPrice?.price,
                                    Gender = doctor.Gender.ToString(),
                                    Specialize = doctor.Specialization?.Title,
                                    Appointments = doctor
                                        .Appointments
                                        ?.Select(
                                            appointment =>
                                                new
                                                {
                                                    Day = appointment.Day.Name,
                                                    Times = appointment
                                                        .Times
                                                        .Select(
                                                            time =>
                                                                time.Time
                                                                    ?.TimeValue
                                                                    .ToString("h:mm tt")
                                                        )
                                                }
                                        )
                                }
                        )
                        .ToList();

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
                                ? doctorInfo.Specialization.Title
                                : null
                        },
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{Id}")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> DeleteDoctor([FromRoute] string Id)
        {
            try
            {
                IdentityResult DoctorResult = await _doctorService.DeleteDoctor(Id);

                if (!DoctorResult.Succeeded)
                {
                    if (DoctorResult.Errors.Any(error => error.Code == "NotFound"))
                    {
                        return NotFound();
                    }
                    if (DoctorResult.Errors.Any(error => error.Code == "NotAuthorized"))
                    {
                        return Forbid();
                    }
                    return BadRequest();
                }

                return Ok(
                    new
                    {
                        success = true,
                        statusCode = 200,
                        message = "Doctor deleted successfully",
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("my-bookings")]
        [Authorize(policy: "DoctorOnly")]
        public async Task<IActionResult> GetMyBookings(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageNumber = 1,
            [FromQuery] DateOnly date = default
        )
        {
            try
            {
                // Access claims from the current user's ClaimsPrincipal
                ClaimsPrincipal user = HttpContext.User;

                // Get the value of a specific claim
                string? doctorId = user.FindFirst("Id")?.Value;
                if (doctorId == null)
                {
                    return Unauthorized();
                }

                PaginatedBookingsDto bookings = await _bookingService.GetBookingsByDoctorIdAsync(
                    doctorId,
                    pageSize,
                    pageNumber,
                    date,
                    ["Patient"]
                );

                if (bookings == null || bookings.TotalBookings == 0)
                {
                    return NotFound("No bookings found for the specified doctor.");
                }

                // format response
                var bookingsResponse = bookings
                    .Bookings
                    .Select(
                        b =>
                            new
                            {
                                PatientNames = b.Patient.FullName,
                                Image = b.Patient.Image, // Replace with actual property
                                Age = b.Patient.DateOfBirth,
                                Gender = b.Patient.Gender.ToString(),
                                Phone = b.Patient.PhoneNumber,
                                Email = b.Patient.Email,
                                Date = b.Date.ToString("d/M/yyyy")
                            }
                    )
                    .ToList();

                return Ok(
                    new
                    {
                        succes = true,
                        statusCode = 200,
                        totalBookingCount = bookings.TotalBookings,
                        maxPages = bookings.MaxPages,
                        currentPage = pageNumber,
                        itemsPerPage = pageSize,
                        bookings = bookingsResponse
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
