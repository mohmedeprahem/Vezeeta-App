using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using AutoMapper;
using Core.Models;

namespace Application.Mappers
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<PatientDto, ApplicationUser>();
            CreateMap<ApplicationUser, PatientDto>();
            CreateMap<DoctorDto, ApplicationUser>();
        }
    }
}
