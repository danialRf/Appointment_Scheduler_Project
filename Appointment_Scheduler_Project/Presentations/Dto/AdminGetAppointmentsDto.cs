using Appointment_Scheduler_Project.Domain.Enums;

namespace Appointment_Scheduler_Project.Presentations.Dto
{
    public class AdminGetAppointmentsDto
    {
        public string? Name { get; set; }
        public string AppointmentName { get; set; } = "Admin";
        public DateTime DateOfBirth { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
    }
}
