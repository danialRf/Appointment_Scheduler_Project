using System.Security.Claims;

namespace Appointment_Scheduler_Project.Infrastructure.Service
{

    public interface IUserService
    {
        Task<Guid> GetCurrentUserIdAsync();

        Task<bool> IsAppointmentBelongingToCurrentUserAsync(int appointmentId);
    }

}
