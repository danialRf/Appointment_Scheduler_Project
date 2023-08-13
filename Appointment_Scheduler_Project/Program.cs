using Appointment_Scheduler_Project.Persistences.EF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
//using Hangfire;
using Appointment_Scheduler_Project.Applications.Repository;
using Appointment_Scheduler_Project.Applications.Services;
using Appointment_Scheduler_Project.Infrastructure;
using Appointment_Scheduler_Project.Persistences.Repositories;
//using Hangfire.SqlServer;
using Appointment_Scheduler_Project.Domain.Entities;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = configurationBuilder.Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);


//builder.Services.AddHangfire(config =>
//{
//    config.UseSqlServerStorage(connectionString);
//});
//JobStorage.Current = new SqlServerStorage(configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddDbContext<ApScDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApScDbContext>()
            .AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();


var serviceProvider = builder.Services.BuildServiceProvider();
var appointmentService = serviceProvider.GetRequiredService<IAppointmentService>(); // Adjust the service type
//RecurringJob.AddOrUpdate("expireAppointments", () => appointmentService.ExpireAppointmentsBeforeToday(), Cron.Daily);


builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
//app.UseHangfireDashboard();





app.UseHttpsRedirection();
app.UseAuthorization();
//app.UseHangfireServer();
app.MapControllers();

app.Run();
