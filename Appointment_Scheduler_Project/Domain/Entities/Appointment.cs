using Appointment_Scheduler_Project.Domain.Enums;

namespace Appointment_Scheduler_Project.Domain.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool IsExpired { get; set; } = false;
        public virtual ApplicationUser ApplicationUser { get; set; }

        public AppointmentStatus AppointmentStatus {get; set; }
    }
}
