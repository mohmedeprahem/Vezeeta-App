using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CreateAppointmentDto
    {
        [Required]
        public int Price { get; set; }

        [Required]
        public List<AppointmentDto> Appointments { get; set; }
    }
}
