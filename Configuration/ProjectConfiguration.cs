using System.Reflection;
using System.Text;
using Carter;
using EManagementVSA.Data;
using EManagementVSA.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EManagementVSA.Configuration;

public static class ProjectConfiguration
{
    public static IServiceCollection AddProjectConfiguration(this IServiceCollection services, IConfiguration config, WebApplicationBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var secretKey =  config.GetSection("JwtSettings").GetValue<string>("Key");

        services.AddDbContext<EmployeeDbContext>(options => {
            options.UseNpgsql(config.GetConnectionString("Default"));
        });

        services.AddLogging();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddCarter();
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(assembly);

        // Identity configuration
        services.AddAuthentication(option => {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
            options.RequireHttpsMetadata = false;
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey ?? string.Empty)),
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddIdentity<Employee, IdentityRole>()
                .AddEntityFrameworkStores<EmployeeDbContext>()
                .AddApiEndpoints();

        builder.Host.UseSerilog((context, loggerConfig) => {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
        });

        return services;
    }
}