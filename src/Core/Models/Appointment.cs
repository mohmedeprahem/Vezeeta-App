using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public int DayId { get; set; }
        public ICollection<AppointmentTime> Times { get; set; }
        public ApplicationUser Doctor { get; set; }
        public Day Day { get; set; }
    }
}
