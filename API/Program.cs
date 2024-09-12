using API.Extension;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Hangfire config
builder.Services.AddHangfire((sp, config) =>
{
    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("SqlCloud");
    config.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();

builder.Services.ApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();
app.MapHangfireDashboard("/hangfire");

using (var scope = app.Services.CreateScope())
{
    //var serviceProvider = scope.ServiceProvider;
    //var backgroundService = serviceProvider.GetRequiredService<IBackgroundService>();

    var timeZoneId = "SE Asia Standard Time";
    var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

    var localTime = new DateTime(1, 1, 1, 17, 30, 0);
    var utcTime = TimeZoneInfo.ConvertTimeToUtc(localTime, timeZone);

    var cronMinute = utcTime.Minute;
    var cronHour = utcTime.Hour;

    //var timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
    RecurringJob.AddOrUpdate("RecurringJob", () => Console.WriteLine("Recurring Job Triggered at " +
        timeZone.DisplayName + TimeZoneInfo.ConvertTime(DateTime.Now, timeZone), timeZone), $"{cronMinute} {cronHour} * * *");

    //RecurringJob.AddOrUpdate("RecurringJob", () => Console.WriteLine("Recurring Job Triggered at TimeZone" + TimeZoneInfo.GetSystemTimeZones(), TimeZoneInfo.Local), "* * * * *");
    //RecurringJob.AddOrUpdate("TestSchedule", () => backgroundService.ScheduleMembershipWhenExpire(), Cron.MinuteInterval(5));
}

app.Run();
