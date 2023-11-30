using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Time
    {
        public int Id { get; set; }
        public string TimeValue { get; set; }
        public ICollection<AppointmentTime> Appointments { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
