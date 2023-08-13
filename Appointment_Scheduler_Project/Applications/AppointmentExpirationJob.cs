using Appointment_Scheduler_Project.Infrastructure;

namespace Appointment_Scheduler_Project.Applications
{
    public class AppointmentExpirationJob
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentExpirationJob(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public void ExpireAppointments()
        {
            var today = DateTime.Today;
            _appointmentService.ExpireAppointmentsBeforeToday().Wait();
        }

    }
}
