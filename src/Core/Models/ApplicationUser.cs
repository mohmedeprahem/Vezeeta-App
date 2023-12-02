using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.enums;
using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public override string Email { get; set; }
        public string? Image { get; set; }
        public int? SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
        public ExaminationPrice Price { get; set; }
        public UserBookingTracking AprovedBookingCount { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Booking> PatientBookings { get; set; }
    }
}
