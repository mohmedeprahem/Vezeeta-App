using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<List<ApplicationUser>> GetUsersByRole(string role, int page = 1, int size = 8);
        public Task<int> GetUsersCountByRole(string role);
    }
}
