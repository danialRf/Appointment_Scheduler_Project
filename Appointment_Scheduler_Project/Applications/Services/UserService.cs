using Appointment_Scheduler_Project.Infrastructure.Service;
using System.Security.Claims;

namespace Appointment_Scheduler_Project.Applications.Services
{
    public class UserService : IUserService
    {
        public async Task<Guid?> GetAdminUserIdAsync(ClaimsPrincipal user)
        {
            var adminUserId = user.FindFirst(ClaimTypes.Sid)?.Value;

            if (Guid.TryParse(adminUserId, out Guid userId))
            {
                return userId;
            }

            return null;
        }
    }
}
