namespace Appointment_Scheduler_Project.Infrastructure.Repositories
{
    public interface IAppointmentService
    {
        Task ExpireAppointmentsBeforeToday();
    }
}
