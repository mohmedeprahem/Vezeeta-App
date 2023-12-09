using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int AppointmentTimeId { get; set; }
        public int BookingStatusId { get; set; }
        public string PatientId { get; set; }
        public int? DiscountId { get; set; }
        public DateOnly Date { get; set; }
        public DateTime CreatedAt { get; private set; }

        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Range(0, int.MaxValue)]
        public int FinalPrice { get; set; }
        public int SpecializationId { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public ApplicationUser Patient { get; set; }
        public Discount Discount { get; set; }
        public AppointmentTime AppointmentTime { get; set; }
        public Specialization Specialization { get; set; }
    }
}
