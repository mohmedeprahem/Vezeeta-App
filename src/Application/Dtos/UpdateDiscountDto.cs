using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UpdateDiscountDto
    {
        [MinLength(6)]
        [MaxLength(6)]
        public string DiscountCode { get; set; }
        public int DiscountTypeId { get; set; }
        public int DiscountValue { get; set; }
    }
}
