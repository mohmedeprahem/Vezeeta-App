using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        public Task<Appointment> CreateAppointmentDayAsync(Appointment appointment);
        public Task<Appointment> GetAppointmentByDayIdAsync(int dayId, string doctorId);
        public Task<AppointmentTime> CreateAppointmentTimeAsync(int appointmentDayId, string time);
        public Task<bool> IsDayAddedBefore(int dayId, string doctorId);
    }
}
