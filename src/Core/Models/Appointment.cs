using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateOnly Date { get; set; }
        public ICollection<AppointmentTime> Times { get; set; }
        public ApplicationUser Doctor { get; set; }
        public Day Day { get; set; }
    }
}
