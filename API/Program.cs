using API.Extension;
using Hangfire;
using HostelManagementWebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using Service;
using Service.Interface;
using Service.SignalRHub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
Log.Information("Hello, {Name}!", Environment.UserName);

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
ConfigurationHelper.Initialize(builder.Configuration);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("SqlCloud")));

//builder.Services.AddHangfireServer(options =>
//{
//    options.Queues = new[] { "default", "critical" };
//});

builder.Services.AddHangfireServer();

builder.Services.AddScoped<IBackgroundService, Service.Implement.BackgroundServiceImpl>();

builder.Services.IdentityServices(builder.Configuration);
builder.Services.ApplicationServices(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();
app.MapHangfireDashboard("/hangfire");

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var backgroundService = serviceProvider.GetRequiredService<IBackgroundService>();

    var timeZoneId = "SE Asia Standard Time";
    var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

    var localTime = new DateTime(1, 1, 1, 0, 0, 00);
    var utcTime = TimeZoneInfo.ConvertTimeToUtc(localTime, timeZone);

    var cronMinute = utcTime.Minute;
    var cronHour = utcTime.Hour;

    //var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
    //RecurringJob.AddOrUpdate("RecurringJob", () => Console.WriteLine("Recurring Job Triggered at " +
    //    timeZone.DisplayName + TimeZoneInfo.ConvertTime(DateTime.Now, timeZone), timeZone), $"{cronMinute} {cronHour} * * *");

    //RecurringJob.AddOrUpdate("PromotionJob",
    //    () => backgroundService.PromotionJob(), $"{cronMinute} {cronHour} * * *");

    RecurringJob.AddOrUpdate(
        "PromotionJob",
        () => backgroundService.PromotionJob(),
        "0 0 * * *");

    //RecurringJob.AddOrUpdate("RecurringJob", () => Console.WriteLine("Recurring Job Triggered at TimeZone" + TimeZoneInfo.GetSystemTimeZones(), TimeZoneInfo.Local), "* * * * *");
    //RecurringJob.AddOrUpdate("TestSchedule", () => backgroundService.ScheduleMembershipWhenExpire(), Cron.MinuteInterval(5));
}

app.MapHub<DeliveryHub>("/delivery");
app.MapHub<EmployeeTaskHub>("/employee-task");
app.MapHub<InvoiceHub>("/invoice");
app.MapHub<MaintenanceRequestHub>("/maintenance-request");
app.MapHub<MaintenanceTicketHub>("/maintenance-ticket");

app.Run();
