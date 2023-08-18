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
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Appointment_Scheduler_Project.Infrastructure;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.IdentityModel.Tokens;
using MyCmsWebApi2.Domain.Entities;
using System.Text;
using Appointment_Scheduler_Project.Infrastructure.HostedServices;
using Microsoft.OpenApi.Models;
using Appointment_Scheduler_Project.Infrastructure.Service;

var builder = WebApplication.CreateBuilder(args);
var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var configuration = configurationBuilder.Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddDbContext<ApScDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


builder.Host.UseSerilog();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog(dispose: true);
});
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File($"C://Users/User/OneDrive/Documents/AppointmentSc. logs/AppointmentSc. log {DateTime.Now:yyyy-MM-dd-}.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();




builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddSingleton<QuartzConfiguration>();

builder.Services.AddScoped<ExpireAppointmentsJob>();// Register the job


#region Authentication
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection(key: "Jwt:Secret"));

var key = Encoding.ASCII.GetBytes(builder.Configuration[key: "Jwt:Key"]!);

var tokenValidationParameter = new TokenValidationParameters
{
    ValidateIssuer = false,
    ValidateAudience = true,
    ValidateLifetime = true,
    RequireExpirationTime = true,
    ValidateIssuerSigningKey = false,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = "FaghatKhooba",
    IssuerSigningKey = new SymmetricSecurityKey(key),
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = tokenValidationParameter;
});

builder.Services.AddSingleton(tokenValidationParameter);

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<ApScDbContext>()
    .AddDefaultTokenProviders()
    .AddRoles<IdentityRole<Guid>>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddSignInManager<SignInManager<ApplicationUser>>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Set the username field to the phone number field
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Set the username field to the phone number field
});
builder.Services.AddTransient<IHostedService, RoleSeederHostedService>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim("role", "admin"));
});
builder.Services.AddScoped<JwtTokenGenerator>();

#endregion



builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();


var serviceProvider = builder.Services.BuildServiceProvider();
var appointmentService = serviceProvider.GetRequiredService<IAppointmentService>(); // Adjust the service type


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AppointmentSchedular", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
        {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
        {
        Type = ReferenceType.SecurityScheme,
        Id = "Bearer"
        }
        },
        new string[] {}
        }
        });
});

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
await InitializeQuartzAsync(app.Services);

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
