using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserBookingTracking
    {
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }
        public int AprovedBookingCount { get; set; }
    }
}
