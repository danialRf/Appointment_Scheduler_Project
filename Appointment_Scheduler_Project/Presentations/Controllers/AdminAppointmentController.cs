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
            var userId = _userService.GetCurrentUserId();
            var appointment = _mapper.Map<Appointment>(appointmentDto);
            //appointment.AppointmentStatus = AppointmentStatus.approved;
            appointment.UserId = userId;
            var result = await _appointmentRepository.Create(appointment);
            _logger.LogInformation("Appointment Created");
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminGetAppointmentDto>>> AdminGetAllFreeAppointments()
        {
            var appointments = await _appointmentRepository.GetAllFreeAppointments();

            var result = _mapper.Map<List<AdminGetAppointmentDto>>(appointments);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminGetAppointmentDto>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentRepository.GetById(id);

            if (appointment == null)
                return NotFound();

            var appointmentDto = _mapper.Map<AdminGetAppointmentDto>(appointment);

            return Ok(appointmentDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentRepository.Delete(id);

            return NoContent();
        }

    }
}
