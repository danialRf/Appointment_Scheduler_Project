using Appointment_Scheduler_Project.Applications.Repository;
using Appointment_Scheduler_Project.Infrastructure;
using Appointment_Scheduler_Project.Persistences.Repositories;
//using Hangfire;

namespace Appointment_Scheduler_Project.Applications.Services
{
    public class AppointmentService : IAppointmentService

    {
        private readonly IAppointmentRepository _appointmentRepository;
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public async Task ExpireAppointmentsBeforeToday()
        {
            var today = DateTime.Today;
            var appointmentsToExpire = _appointmentRepository.GetAppointmentsBeforeDate(today);
            foreach (var appointment in await appointmentsToExpire)
            {
                appointment.IsExpired = true;
                await _appointmentRepository.Update(appointment);
            }
           
        }
    }
}
