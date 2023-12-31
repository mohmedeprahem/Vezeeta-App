﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Discount
    {
        public int Id { get; set; }

        [Range(6, 6)]
        public string DiscountCode { get; set; }
        public int DiscountTypeId { get; set; }
        public bool IsActivated { get; set; }
        public int DiscountValue { get; set; }
        public DiscountType DiscountType { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
