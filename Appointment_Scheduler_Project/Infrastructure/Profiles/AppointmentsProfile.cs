﻿using Appointment_Scheduler_Project.Domain.Entities;
using Appointment_Scheduler_Project.Presentations.Dto;
using AutoMapper;

namespace Appointment_Scheduler_Project.Infrastructure.Profiles
{
    public class AppointmentsProfile : Profile
    {
        public AppointmentsProfile()
        {
            CreateMap<Appointment, UserGetAppointmentsDto>();
            CreateMap<UserGetAppointmentsDto, Appointment>();

            CreateMap<UserAddAppointmentDto, Appointment>();
            CreateMap<Appointment,UserAddAppointmentDto > ();

        }
    }
}
