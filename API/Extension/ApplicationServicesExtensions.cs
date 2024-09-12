using Repository.Implement;
using Repository.Interface;
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

        //Service
        services.AddScoped<IAccountService, AccountServiceImpl>();
        services.AddScoped<IProductService, ProductServiceImpl>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //the current position of the mapping profile

        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
        });


        return services;

    }
}