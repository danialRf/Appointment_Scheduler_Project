using Appointment_Scheduler_Project.Applications.Repository;
using Appointment_Scheduler_Project.Domain.Entities;
using Appointment_Scheduler_Project.Infrastructure.Service;
using Appointment_Scheduler_Project.Persistences.EF;
using Appointment_Scheduler_Project.Presentations.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Appointment_Scheduler_Project.Presentations.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize]
    public class UserAppointmentsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApScDbContext _dbContext;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserAppointmentsController(UserManager<ApplicationUser> userManager, ApScDbContext dbContext, IAppointmentRepository appointmentRepository, IMapper mapper, IUserService userService)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
            _userService = userService;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserGetAppointmentsDto>>> UserGetAllFreeAppointments()
        {
            var appointments = await _appointmentRepository.GetAllFreeAppointments();

            var result = _mapper.Map<List<UserGetAppointmentsDto>>(appointments);

            return Ok(result);
        }

        //[HttpGet("UserGetMyAppointment")]
        //public async Task<ActionResult<IEnumerable<UserGetAppointmentsDto>>> UserGetMyAppointments()
        //{
        //    var appointments = await _appointmentRepository.GetAll();

        //    List<Appointment> result = new List<Appointment>();

        //    foreach (var appointment in appointments)
        //    {

        //        if (await _userService.IsAppointmentBelongingToCurrentUserAsync(appointment.Id) == true)
        //        {
        //            result.Add(appointment);
        //        }
        //        _mapper.Map<Appointment>(result);
        //    }

        //    return Ok(result);
        //}

        [HttpPost]
        public async Task<ActionResult> UserAddAppointment([FromBody] UserAddAppointmentDto userAddAppointment)
        {

            var userId = _userService.GetCurrentUserIdAsync();
            var appointmentId = await _appointmentRepository.GetAppointmentIdByDate(userAddAppointment.AppontmentDate);

            var appointment = _mapper.Map<Appointment>(userAddAppointment);
            if (await _appointmentRepository.Isvalid(appointment.Id))
            {
                appointment.Id = appointmentId;
                appointment.UserId = userId.Result;
                appointment.AppointmentName = "User";
                appointment.IsExpired = true;
                appointment.AppointmentDate = userAddAppointment.AppontmentDate;
                var result = await _appointmentRepository.Update(appointment);

                return Ok(result);
            }
            return BadRequest("The date you entered for appointment is not valid");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> UserDeleteAppointment(int id)
        {
            //var appointment = await _appointmentRepository.GetById(id);
            bool appointmentBelongingResult = await _userService.IsAppointmentBelongingToCurrentUserAsync(id);
            if (appointmentBelongingResult == false)
            {
                var appointment = await _appointmentRepository.Delete(id);


            }
            return NoContent();
        }

    }
}
