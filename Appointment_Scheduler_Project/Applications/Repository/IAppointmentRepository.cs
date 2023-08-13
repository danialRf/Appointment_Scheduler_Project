using Appointment_Scheduler_Project.Domain.Entities;
using Appointment_Scheduler_Project.Infrastructure;

namespace Appointment_Scheduler_Project.Applications.Repository
{
    public interface IAppointmentRepository:IRepository<Appointment,int>
    {
        Task<IEnumerable<Appointment>> GetAllFreeAppointments();

        Task <bool> Isvalid(int appointmentId);

        Task<bool> Expire (int appointmentId);

        Task<List<Appointment>> GetAppointmentsBeforeDate(DateTime date);
    }
}
