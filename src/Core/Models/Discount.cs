using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public string DiscountCode { get; set; }
        public int DiscountTypeId { get; set; }
        public bool IsActivated { get; set; }
        public int DiscountValue { get; set; }
        public DiscountType DiscountType { get; set; }
    }
}
