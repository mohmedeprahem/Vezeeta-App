using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Application.Interfaces.Repositories
{
    public interface ISpecializationRepository
    {
        public Task<Specialization> GetSpecializationByTitle(string title);
    }
}
