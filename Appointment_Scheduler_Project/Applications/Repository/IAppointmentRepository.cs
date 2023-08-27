using Appointment_Scheduler_Project.Domain.Entities;
using Appointment_Scheduler_Project.Infrastructure.Repositories;

namespace Appointment_Scheduler_Project.Applications.Repository
{
    public interface IAppointmentRepository:IRepository<Appointment,int>
    {
        Task<IEnumerable<Appointment>> GetAllFreeAppointments();

        Task <bool> Isvalid(int appointmentId);

       Task<int> GetAppointmentIdByDate(DateTime date);

        Task<List<Appointment>> GetAppointmentsBeforeDate(DateTime date);
    }
}
