using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ExaminationPrice
    {
        public string DoctorId { get; set; }
        public ApplicationUser Doctor { get; set; }
        public int price { get; set; }
    }
}
