using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.enums;

namespace Core.Models
{
    public class DiscountType
    {
        public int Id { get; set; }
        public DiscountTypeEnum Name { get; set; }
        public ICollection<Discount> Discounts { get; set; }
    }
}
