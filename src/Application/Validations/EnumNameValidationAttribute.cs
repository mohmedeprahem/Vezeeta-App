using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false
    )]
    public class EnumNameValidationAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumNameValidationAttribute(Type enumType) => _enumType = enumType;

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext
        )
        {
            if (value != null)
            {
                string enumName = value.ToString();

                if (Enum.IsDefined(_enumType, enumName))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(
                        $"The {validationContext.DisplayName} field must have a valid enum name."
                    );
                }
            }

            return ValidationResult.Success;
        }
    }
}
