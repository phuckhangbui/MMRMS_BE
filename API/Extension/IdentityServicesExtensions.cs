using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HostelManagementWebAPI.Extensions;

public static class IdentityServiceExtension
{

    const string ADMIN_ID = "0";
    const string MANAGER_ID = "1";
    const string STAFF_ID = "2";
    const string CUSTOMER_ID = "3";
    public static IServiceCollection IdentityServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
                      policy.RequireClaim("RoleId", ADMIN_ID));
            options.AddPolicy("Manager", policy =>
                      policy.RequireClaim("RoleId", MANAGER_ID));
            options.AddPolicy("Staff", policy =>
                      policy.RequireClaim("RoleId", STAFF_ID));
            options.AddPolicy("ManagerAndStaff", policy =>
                        policy.RequireClaim("RoleId", MANAGER_ID, STAFF_ID));
            options.AddPolicy("Customer", policy =>
                        policy.RequireClaim("RoleId", CUSTOMER_ID));
        });
        return services;
    }
}