﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserBookingTracking
    {
        public string PatientId { get; set; }
        public ApplicationUser Patient { get; set; }

        [Range(0, int.MaxValue)]
        public int AprovedBookingCount { get; set; }
    }
}
