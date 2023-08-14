using Appointment_Scheduler_Project.Infrastructure.Configurations.YourAppNamespace.Infrastructure.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace Appointment_Scheduler_Project.Infrastructure
{
    public class QuartzConfiguration
    {
        public async Task Initialize(IServiceProvider serviceProvider)
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            IJobDetail jobDetail = JobBuilder.Create<ExpireAppointmentsJob>()
                .WithIdentity("ExpireAppointmentsJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("ExpireAppointmentsTrigger")
                .StartNow()
                .WithCronSchedule("0 0 0 * * ?") // Daily at midnight
                .Build();

            scheduler.JobFactory = new CustomJobFactory(serviceProvider);
            await scheduler.ScheduleJob(jobDetail, trigger);

            await scheduler.Start();
        }
    }
}
