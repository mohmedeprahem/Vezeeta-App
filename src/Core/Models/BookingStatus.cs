using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.enums;

namespace Core.Models
{
    public class BookingStatus
    {
        public int Id { get; set; }
        public BookingStatusEnum Name { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
