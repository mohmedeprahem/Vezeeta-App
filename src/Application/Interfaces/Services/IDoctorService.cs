using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Application.Interfaces.Services
{
    public interface IDoctorService
    {
        public Task<List<ApplicationUser>> GetDoctors(int page, int size, string search);
        public Task<int> GetDoctorsCount(string lastDate = "");
    }
}
