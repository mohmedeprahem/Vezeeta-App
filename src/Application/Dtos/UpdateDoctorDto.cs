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
    public class UpdateDoctorDto
    {
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [EnumNameValidation(typeof(GendersEnum))]
        public string Gender { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public int SpecializationId { get; set; }
        public IFormFile? Image { get; set; }
        public string OldImage { get; set; }
    }
}
