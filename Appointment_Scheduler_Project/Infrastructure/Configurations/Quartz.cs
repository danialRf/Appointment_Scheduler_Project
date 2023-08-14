namespace Appointment_Scheduler_Project.Infrastructure.Configurations
{
    using Quartz;
    using Quartz.Spi;
    using System;

    namespace YourAppNamespace.Infrastructure.Quartz // Adjust the namespace
    {
        public class CustomJobFactory : IJobFactory
        {
            private readonly IServiceProvider _serviceProvider;

            public CustomJobFactory(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            {
                return (IJob)_serviceProvider.GetService(bundle.JobDetail.JobType);
            }

            public void ReturnJob(IJob job) { }
        }
    }

}
