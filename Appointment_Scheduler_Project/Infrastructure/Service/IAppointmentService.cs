namespace Appointment_Scheduler_Project.Infrastructure.Service
{
    public interface IAppointmentService
    {
        Task ExpireAppointmentsBeforeToday();
    }
}
