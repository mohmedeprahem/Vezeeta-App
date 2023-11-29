using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName;
        public string LastName;
        public string Email;
        public string Gender;
        public DateTime DateOfBirth;
        public string? Image;
    }
}
