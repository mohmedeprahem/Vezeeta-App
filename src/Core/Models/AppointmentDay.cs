using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AppointmentDay
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
