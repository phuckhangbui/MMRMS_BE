using Common.Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extension;

public static class IdentityServiceExtension
{
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
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
         policy.RequireClaim("RoleId", ((int)AccountRoleEnum.Admin).ToString()));

            options.AddPolicy("Manager", policy =>
                policy.RequireClaim("RoleId", ((int)AccountRoleEnum.Manager).ToString()));

            options.AddPolicy("Employee", policy =>
                policy.RequireClaim("RoleId", ((int)AccountRoleEnum.Manager).ToString(), ((int)AccountRoleEnum.TechnicalStaff).ToString(), ((int)AccountRoleEnum.WebsiteStaff).ToString()));

            options.AddPolicy("TechnicalStaff", policy =>
                policy.RequireClaim("RoleId", ((int)AccountRoleEnum.TechnicalStaff).ToString()));

            options.AddPolicy("WebsiteStaff", policy =>
                policy.RequireClaim("RoleId", ((int)AccountRoleEnum.WebsiteStaff).ToString()));

            options.AddPolicy("ManagerAndTechnicalStaff", policy =>
                policy.RequireClaim("RoleId", ((int)AccountRoleEnum.Manager).ToString(), ((int)AccountRoleEnum.TechnicalStaff).ToString()));

            options.AddPolicy("CustomerAndTechnicalStaff", policy =>
                policy.RequireClaim("RoleId", ((int)AccountRoleEnum.Customer).ToString(), ((int)AccountRoleEnum.TechnicalStaff).ToString()));

            options.AddPolicy("ManagerAndCustomer", policy =>
                policy.RequireClaim("RoleId", ((int)AccountRoleEnum.Manager).ToString(), ((int)AccountRoleEnum.Customer).ToString()));

            options.AddPolicy("Customer", policy =>
                policy.RequireClaim("RoleId", ((int)AccountRoleEnum.Customer).ToString()));

            options.AddPolicy("AdminAndManager", policy =>
                policy.RequireClaim("RoleId", ((int)AccountRoleEnum.Admin).ToString(), ((int)AccountRoleEnum.Manager).ToString()));
        });
        return services;
    }
}