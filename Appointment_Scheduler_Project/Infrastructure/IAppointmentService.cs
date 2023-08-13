namespace Appointment_Scheduler_Project.Infrastructure
{
    public interface IAppointmentService
    {
        Task ExpireAppointmentsBeforeToday();
    }
}
