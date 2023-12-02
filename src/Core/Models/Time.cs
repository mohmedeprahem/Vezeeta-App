using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Time
    {
        public int Id { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:h:mm tt}")]
        public TimeOnly TimeValue { get; set; }
        public ICollection<AppointmentTime> Appointments { get; set; }
    }
}
