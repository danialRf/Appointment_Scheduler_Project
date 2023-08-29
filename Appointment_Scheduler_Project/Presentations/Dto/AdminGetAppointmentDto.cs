using Appointment_Scheduler_Project.Domain.Enums;

namespace Appointment_Scheduler_Project.Presentations.Dto
{
    public class AdminGetAppointmentDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string AppointmentName { get; set; } = "Admin";
        public DateTime DateOfBirth { get; set; }
        public DateTime AppointmentDate { get; set; }

        public bool IsExpired { get; set; }
        
    }
}
