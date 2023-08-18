using Appointment_Scheduler_Project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Appointment_Scheduler_Project.Persistences.EF.ModelMap
{
    public class AppointmentMap : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointment");

            builder.HasKey(p => p.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(p => p.ApplicationUser)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
