using Appointment_Scheduler_Project.Applications.Repository;
using Appointment_Scheduler_Project.Domain.Entities;
using Appointment_Scheduler_Project.Domain.Enums;
using Appointment_Scheduler_Project.Persistences.EF;
using Appointment_Scheduler_Project.Presentations.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Appointment_Scheduler_Project.Presentations.Controllers
{

    [Route("api/[controller]")]
    [ApiController]


    public class AdminAppointmentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApScDbContext _dbContext;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AdminAppointmentController(UserManager<ApplicationUser> userManager, ApScDbContext dbContext, IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult> AdminAddAppointments([FromBody] AdminSetApointmentDto appointmentDto)
        {
            var news = _mapper.Map<Appointment>(appointmentDto);
            news.AppointmentStatus = AppointmentStatus.approved;
            var result = _appointmentRepository.Create(news);
            return Ok(result);
        }
    }
}
