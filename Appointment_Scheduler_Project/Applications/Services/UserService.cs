using Appointment_Scheduler_Project.Applications.Repository;
using Appointment_Scheduler_Project.Domain.Entities;
using Appointment_Scheduler_Project.Infrastructure.Service;
using System.Security.Claims;

namespace Appointment_Scheduler_Project.Applications.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppointmentRepository _appointmentRepository;
        public UserService(IHttpContextAccessor httpContextAccessor, IAppointmentRepository appointmentRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Guid> GetCurrentUserIdAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid);

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return userId;
            }

            throw new InvalidOperationException("Invalid UserId");
        }
        public async Task<bool> IsAppointmentBelongingToCurrentUserAsync(int appointmentId)
        {
            Guid currentUserId = await GetCurrentUserIdAsync();

            Appointment appointment = await _appointmentRepository.GetById(appointmentId);

            if (appointment != null && appointment.UserId == currentUserId)
            {
                return true;
            }

            return false;
        }
    }
}
