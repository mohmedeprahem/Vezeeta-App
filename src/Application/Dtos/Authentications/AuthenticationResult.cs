using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Application.Dtos.Authentications
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
