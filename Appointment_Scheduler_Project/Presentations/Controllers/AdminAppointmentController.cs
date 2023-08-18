using Appointment_Scheduler_Project.Applications.Repository;
using Appointment_Scheduler_Project.Domain.Entities;
using Appointment_Scheduler_Project.Domain.Enums;
using Appointment_Scheduler_Project.Infrastructure.Service;
using Appointment_Scheduler_Project.Persistences.EF;
using Appointment_Scheduler_Project.Presentations.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Appointment_Scheduler_Project.Presentations.Controllers
{

    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]

    public class AdminAppointmentController : ControllerBase
    {
        private readonly ILogger<AdminAppointmentController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApScDbContext _dbContext;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        

        public AdminAppointmentController(UserManager<ApplicationUser> userManager, ApScDbContext dbContext, IAppointmentRepository appointmentRepository, IMapper mapper, ILogger<AdminAppointmentController> logger, IUserService userService)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
        }


        [HttpPost]
        public async Task<ActionResult> AdminAddAppointments([FromBody] AdminSetApointmentDto appointmentDto)
        {

            var adminUserId = await _userService.GetAdminUserIdAsync(User);

            if (adminUserId == null)
            {
                // Handle invalid UserId if needed
                return BadRequest("Invalid UserId");
            }

            var appointment = _mapper.Map<Appointment>(appointmentDto);
            appointment.AppointmentStatus = AppointmentStatus.approved;
            appointment.UserId = adminUserId.Value;

            var result = await _appointmentRepository.Create(appointment);
            _logger.LogInformation("Appointment Created");
            return Ok(result);
        }
    }
}
