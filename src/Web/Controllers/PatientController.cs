using System.Collections.Generic;
using System.Security.Claims;
using Application.Dtos;
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
        private readonly IBookingService _bookingService;

        public PatientController(IPatientService patientService, IBookingService bookingService)
        {
            this._patientService = patientService;
            this._bookingService = bookingService;
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
                int totalPatientsCount = await _patientService.GetPatientCountByStringAsync(search);

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
                PatientAllInfoDto patientsInfo = await _patientService.GetAllInfoPatientById(id);

                if (patientsInfo == null)
                {
                    return NotFound();
                }

                return Ok(
                    new
                    {
                        succes = true,
                        statusCode = 200,
                        patient = patientsInfo
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("bookings")]
        [Authorize(policy: "PatientOnly")]
        public async Task<IActionResult> GetPatientBookings()
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

                List<Booking> bookingInfo = await _bookingService.GetBookingsByPatientIdAsync(
                    userId,

                    [
                        "AppointmentTime",
                        "BookingStatus",
                        "Specialization",
                        "Discount",
                        "AppointmentTime.Time",
                        "AppointmentTime.Appointment",
                        "AppointmentTime.Appointment.Doctor",
                        "AppointmentTime.Appointment.Day",
                    ]
                );

                if (bookingInfo == null)
                {
                    return NotFound();
                }

                // Format response
                var bookingResponse = bookingInfo.Select(
                    booking =>
                        new
                        {
                            Image = booking.AppointmentTime.Appointment.Doctor.Image,
                            DoctorName = booking.AppointmentTime.Appointment.Doctor.FirstName,
                            Specialize = booking.Specialization.Title,
                            Day = booking.AppointmentTime.Appointment.Day.ToString(),
                            Time = booking.AppointmentTime.Time.TimeValue.ToString("h:mm tt"),
                            Price = booking.Price,
                            DiscountCode = booking.Discount?.DiscountCode,
                            FinalPrice = booking.FinalPrice,
                            Status = booking.BookingStatus.Name.ToString()
                        }
                );

                return Ok(
                    new
                    {
                        succes = true,
                        statusCode = 200,
                        bookings = bookingResponse
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
