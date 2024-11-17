using Repository.Implement;
using Repository.Interface;
using Service;
using Service.Cloundinary;
using Service.Implement;
using Service.Interface;
using Service.Mail;

namespace API.Extension;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection ApplicationServices(this IServiceCollection services
        , IConfiguration config)
    {
        //Repository
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IMachineRepository, MachineRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IContentRepository, ContentRepository>();
        services.AddScoped<IComponentRepository, ComponentRepository>();
        services.AddScoped<IMembershipRankRepository, MembershipRankRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<IRentingRequestRepository, RentingRequestRepository>();
        services.AddScoped<IMachineSerialNumberRepository, MachineSerialNumberRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IRentingServiceRepository, RentingServiceRepository>();
        services.AddScoped<IDeliveryTaskRepository, DeliveryTaskRepository>();
        services.AddScoped<IMachineTaskRepository, MachineTaskRepository>();
        services.AddScoped<IMachineCheckRequestRepository, MachineCheckRequestRepository>();
        services.AddScoped<IComponentReplacementTicketRepository, ComponentReplacementTicketRepository>();
        services.AddScoped<IAccountLogRepository, AccountLogRepository>();
        services.AddScoped<ITermRepository, TermRepository>();
        //services.AddScoped<IRequestResponseRepository, RequestResponseRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IMachineSerialNumberComponentRepository, MachineSerialNumberComponentRepository>();
        services.AddScoped<IMachineSerialNumberLogRepository, MachineSerialNumberLogRepository>();

        //Service
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPayOSService, PayOSService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAccountService, AccountServiceImpl>();
        services.AddScoped<IMachineService, MachineServiceImpl>();
        services.AddScoped<ICategoryService, CategoryServiceImpl>();
        services.AddScoped<IContentService, ContentServiceImpl>();
        services.AddScoped<IComponentService, ComponentServiceImpl>();
        services.AddScoped<IMembershipRankService, MembershipRankServiceImpl>();
        services.AddScoped<IDashboardService, DashboardServiceImpl>();
        services.AddScoped<IContractService, ContractServiceImpl>();
        services.AddScoped<IMachineSerialNumberService, MachineSerialNumberService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IRentingRequestService, RentingRequestServiceImpl>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAddressService, AddressServiceImpl>();
        services.AddScoped<IRentingServiceService, RentingServiceServiceImpl>();
        services.AddScoped<IDeliverService, DeliveryService>();
        services.AddScoped<IMachineTaskService, MachineTaskService>();
        services.AddScoped<IMachineCheckRequestService, MachineCheckRequestService>();
        services.AddScoped<IComponentReplacementTicketService, ComponentReplacementTicketService>();
        services.AddScoped<ILogSerevice, LogSereviceImpl>();
        services.AddScoped<ITermService, TermService>();
        services.AddScoped<IRequestResponseService, RequestResponseService>();
        services.AddScoped<IRoleService, RoleServiceImpl>();
        services.AddScoped<ISettingsService, SettingsService>();


        //Background
        services.AddScoped<IBackground, BackgroundImpl>();
        //services.AddScoped<RentingRequestDao>();
        //services.AddScoped<ContractDao>();
        //services.AddScoped<InvoiceDao>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy
                .WithOrigins("http://localhost:5173")
                .WithOrigins("http://localhost:5174")
                .WithOrigins("https://capstone-project-fe-management.vercel.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });

        services.Configure<CloudinarySetting>(config.GetSection("CloudinarySettings"));
        services.Configure<MailSetting>(config.GetSection("MailSetting"));
        services.AddScoped<ICloudinaryService, CloudinaryService>();

        services.AddSingleton<IFirebaseMessagingService>(provider =>
        {
            // Assuming jsonCredentialsPath is configured elsewhere, possibly in appsettings.json
            var jsonCredentialsPath = config["Firebase:CredentialsPath"];
            return new FirebaseMessagingService(jsonCredentialsPath);
        });

        return services;

    }
}