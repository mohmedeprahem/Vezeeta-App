using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AppointmentTime
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int TimeId { get; set; }
        public bool IsBooked { get; set; }
        public Appointment Appointment { get; set; }
        public Time Time { get; set; }
        public ICollection<Booking> Booking { get; set; }
    }
}
