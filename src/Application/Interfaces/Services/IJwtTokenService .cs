using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Application.Interfaces.Services
{
    public interface IJwtTokenService
    {
        public string GenerateToken(ApplicationUser user, string role);
    }
}
