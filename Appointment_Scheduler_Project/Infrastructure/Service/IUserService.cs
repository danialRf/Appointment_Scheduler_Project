using System.Security.Claims;

namespace Appointment_Scheduler_Project.Infrastructure.Service
{
        
        public interface IUserService
    {
            Task<Guid?> GetAdminUserIdAsync(ClaimsPrincipal user);
        }
    
}
