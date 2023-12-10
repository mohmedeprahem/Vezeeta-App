using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class PatientAllInfoDto
    {
        public string Image { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public List<BookingDto> Requests { get; set; }
    }
}
