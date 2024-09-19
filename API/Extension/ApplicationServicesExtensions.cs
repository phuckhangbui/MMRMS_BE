using Repository.Implement;
using Repository.Interface;
using Service.Cloundinary;
using Service.Implement;
using Service.Interface;

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

        //Service
        services.AddScoped<IAccountService, AccountServiceImpl>();
        services.AddScoped<IProductService, ProductServiceImpl>();
        services.AddScoped<ICategoryService, CategoryServiceImpl>();
        services.AddScoped<IContentService, ContentServiceImpl>();
        services.AddScoped<IComponentService, ComponentServiceImpl>();
        services.AddScoped<IPromotionService, PromotionServiceImpl>();
        services.AddScoped<IMembershipRankService, MembershipRankServiceImpl>();
        services.AddScoped<IDashboardService, DashboardServiceImpl>();

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
        services.AddScoped<ICloudinaryService, CloudinaryService>();

        return services;

    }
}