using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int BookingStatusId { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public int? DiscountCodeId { get; set; }
        public int DayId { get; set; }
        public int TimeId { get; set; }
        public DateTime Date { get; set; }
        public int CreatedAt { get; set; }
        public int Price { get; set; }
        public int FinalPrice { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public ApplicationUser Doctor { get; set; }
        public ApplicationUser Patient { get; set; }
        public Discount DiscountCode { get; set; }
        public AppointmentDay Day { get; set; }
        public Time Time { get; set; }
    }
}
