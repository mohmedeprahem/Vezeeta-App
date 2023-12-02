using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services;

namespace Infrastructure.Services
{
    internal class EnumService : IEnumService
    {
        public string GetEnumCheckConstraint<TEnum>()
            where TEnum : Enum
        {
            // Get names of enum values and join them into a comma-separated string
            string[] enumNames = Enum.GetNames(typeof(TEnum));
            string enumNamesString = string.Join(", ", enumNames);

            // Create the check constraint string
            return $"Gender in ({enumNamesString})";
        }
    }
}
