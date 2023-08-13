using Microsoft.AspNetCore.Identity;

namespace Appointment_Scheduler_Project.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public virtual ICollection <Appointment> Appointments { get; set; }  

    }
}
