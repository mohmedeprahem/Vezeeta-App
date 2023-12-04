using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IEnumService
    {
        public string GetEnumCheckConstraint<TEnum>(string property)
            where TEnum : Enum;
    }
}
