using Repository.Implement;
using Repository.Interface;
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
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IContentRepository, ContentRepository>();
        services.AddScoped<IComponentRepository, ComponentRepository>();
        services.AddScoped<IPromotionRepository, PromotionRepository>();
        services.AddScoped<IMembershipRankRepository, MembershipRankRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<IRentingRepository, RentingRepository>();
        services.AddScoped<ISerialNumberProductRepository, SerialNumberProductRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IRentingServiceRepository, RentingServiceRepository>();
        services.AddScoped<IAccountPromotionRepository, AccountPromotionRepository>();

        //Service
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<ITokenService, TokenService>();
        //services.AddScoped<IFirebaseMessagingService, FirebaseMessagingService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAccountService, AccountServiceImpl>();
        services.AddScoped<IProductService, ProductServiceImpl>();
        services.AddScoped<ICategoryService, CategoryServiceImpl>();
        services.AddScoped<IContentService, ContentServiceImpl>();
        services.AddScoped<IComponentService, ComponentServiceImpl>();
        services.AddScoped<IPromotionService, PromotionServiceImpl>();
        services.AddScoped<IMembershipRankService, MembershipRankServiceImpl>();
        services.AddScoped<IDashboardService, DashboardServiceImpl>();
        services.AddScoped<IContractService, ContractServiceImpl>();
        services.AddScoped<ISerialNumberProductService, SerialNumberProductService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IRentingRequestService, RentingRequestServiceImpl>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAddressService, AddressServiceImpl>();
        services.AddScoped<IRentingServiceService, RentingServiceServiceImpl>();
        services.AddScoped<IAccountPromotionService, AccountPromotionServiceImpl>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy
                .WithOrigins("http://localhost:5173")
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