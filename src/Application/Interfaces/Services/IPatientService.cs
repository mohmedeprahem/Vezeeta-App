using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Application.Interfaces.Services
{
    public interface IPatientService
    {
        public Task<List<ApplicationUser>> GetPatients(int page, int size);
        public Task<int> GetPatientsCount();
    }
}
