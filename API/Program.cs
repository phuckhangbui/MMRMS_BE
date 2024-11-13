using API.Extension;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Service;
using Service.Helper;
using Service.SignalRHub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

builder.Services.IdentityServices(builder.Configuration);
builder.Services.ApplicationServices(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.

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
    var backgroundService = serviceProvider.GetRequiredService<IBackground>();
}

app.MapHub<DeliveryTaskHub>("/delivery-task");
app.MapHub<MachineTaskHub>("/machine-task");
app.MapHub<InvoiceHub>("/invoice");
app.MapHub<MachineCheckRequestHub>("/machine-check-request");
app.MapHub<ComponentReplacementTicketHub>("/component-replacement-ticket");

app.Run();
