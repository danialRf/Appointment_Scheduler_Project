using Appointment_Scheduler_Project.Domain.Entities;
using Appointment_Scheduler_Project.Persistences.EF;
using Appointment_Scheduler_Project.Persistences.Repositories;
using Appointment_Scheduler_Project.Presentations.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Appointment_Scheduler_Project.Presentations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAppointmentsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApScDbContext _dbContext;
        private readonly AppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        public UserAppointmentsController(UserManager<ApplicationUser> userManager, ApScDbContext dbContext, AppointmentRepository appointmentRepository, IMapper mapper)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }


        [HttpGet]

        public async Task<ActionResult<IEnumerable<UserGetAppointmentsDto>>> UserGetAllFreeAppointments()
        {
            var appointments = await _appointmentRepository.GetAllFreeAppointments();
            
            var result = _mapper.Map<List<UserAddAppointmentDto>>(appointments);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> UserAddAppointment([FromBody] UserAddAppointmentDto addAppointment)
        {

            var appointment = _mapper.Map<Appointment>(addAppointment);
            if (await _appointmentRepository.Isvalid(appointment.Id))
            {
                var result = await _appointmentRepository.Create(appointment);
                _appointmentRepository.Expire(appointment.Id);
                return Ok(result);
            }
                return BadRequest("The date you entered for appointment is not valid");
        }

      //  public async Task<ActionResult> UserEditNews();





    }
}
