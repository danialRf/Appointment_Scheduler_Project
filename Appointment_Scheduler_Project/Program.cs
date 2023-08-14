using Appointment_Scheduler_Project.Persistences.EF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Appointment_Scheduler_Project.Applications.Repository;
using Appointment_Scheduler_Project.Applications.Services;
using Appointment_Scheduler_Project.Persistences.Repositories;
using Appointment_Scheduler_Project.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Appointment_Scheduler_Project.Infrastructure.Repositories;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Appointment_Scheduler_Project.Infrastructure;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = configurationBuilder.Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);




builder.Services.AddDbContext<ApScDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<ApScDbContext>()
            .AddDefaultTokenProviders();

builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddScoped<ExpireAppointmentsJob>();// Register the job

builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();


var serviceProvider = builder.Services.BuildServiceProvider();
var appointmentService = serviceProvider.GetRequiredService<IAppointmentService>(); // Adjust the service type


builder.Services.AddSwaggerGen();

static async Task InitializeQuartzAsync(IServiceProvider serviceProvider)
{
    var quartzConfiguration = serviceProvider.GetRequiredService<QuartzConfiguration>();
    using (var scope = serviceProvider.CreateScope())
    {
        var scopedProvider = scope.ServiceProvider;
        await quartzConfiguration.Initialize(scopedProvider);
    }
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




InitializeQuartzAsync(app.Services);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
