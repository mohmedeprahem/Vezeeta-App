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
        public Task<List<ApplicationUser>> GetUsersByRole(
            string role,
            int page,
            int size,
            string search
        );
        public Task<int> GetUsersCountByRole(string role, string lastDate);
        public Task<ApplicationUser> GetUserById(string id);
        public Task<ApplicationUser> GetUserById(string id, string?[] includes);
    }
}
