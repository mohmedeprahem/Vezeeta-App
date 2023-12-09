using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        public Task<IdentityResult> CreateAppointmentAsync(
            List<AppointmentDto> createAppointmentDto,
            string doctorId,
            int price
        );

        public Task<IdentityResult> UpdateAppointmentTimeAsync(
            string doctorId,
            UpdateAppointmentTimeDto updateAppointmentTimeDto,
            int AppointmentTimeId
        );
        public Task<IdentityResult> DeleteAppointmentTimeAsync(
            string doctorId,
            int AppointmentTimeId
        );
    }
}
