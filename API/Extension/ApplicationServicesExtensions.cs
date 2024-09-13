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
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        //Service
        services.AddScoped<IAccountService, AccountServiceImpl>();
        services.AddScoped<IProductService, ProductServiceImpl>();
        services.AddScoped<ICategoryService, CategoryServiceImpl>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //the current position of the mapping profile

        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy => { policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod(); });
        });


        return services;

    }
}