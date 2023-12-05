using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Validations;
using Core.enums;
using Microsoft.AspNetCore.Http;

namespace Application.Dtos
{
    public class RegisterDto
    {
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [EnumNameValidation(typeof(GendersEnum))]
        public string Gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public IFormFile? Image { get; set; }
    }
}
