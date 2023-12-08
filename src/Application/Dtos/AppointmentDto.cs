using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Validations;
using Core.enums;

namespace Application.Dtos
{
    public class AppointmentDto
    {
        [Required]
        [EnumNameValidation(typeof(DaysEnum))]
        public string Day { get; set; }

        [Required]
        public List<string> Times { get; set; }
    }
}
