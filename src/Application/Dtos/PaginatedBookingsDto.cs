using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Application.Dtos
{
    public class PaginatedBookingsDto
    {
        public int TotalBookings { get; set; }
        public int MaxPages { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
