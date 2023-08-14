using Appointment_Scheduler_Project.Infrastructure.Repositories;
using Quartz;
using System;
using System.Threading.Tasks;

public class ExpireAppointmentsJob : IJob
{
    private readonly IAppointmentService _appointmentService;

    public ExpireAppointmentsJob(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _appointmentService.ExpireAppointmentsBeforeToday();
    }
}