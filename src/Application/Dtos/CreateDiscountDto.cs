using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class CreateDiscountDto
    {
        [MinLength(6)]
        [MaxLength(6)]
        public string DiscountCode { get; set; }
        public int DiscountTypeId { get; set; }
        public bool IsActivated { get; set; }
        public int DiscountValue { get; set; }
    }
}
