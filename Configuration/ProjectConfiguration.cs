using System.Reflection;
using System.Text;
using EManagementVSA.Data;
using EManagementVSA.Entities;
using EManagementVSA.Features.Department.GetSubDepartment;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Infrastructure.Mail;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace EManagementVSA.Configuration;

public static class ProjectConfiguration
{
    public static IServiceCollection AddProjectConfiguration(this IServiceCollection services, IConfiguration config, WebApplicationBuilder builder)
    {
        
        var assembly = Assembly.GetExecutingAssembly();
        var jwtSettings = config.GetSection("JwtSettings");
        var secretKey =  jwtSettings.GetValue<string>("Key");

        License.LicenseKey = config.GetValue<string>("IronPdfLicenseKey");

        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseNpgsql(config.GetConnectionString("Default"),
            options => options.MigrationsHistoryTable("EmployeeManagementHistory"));
        });

        services.AddLogging();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddCarter();
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(assembly);

        services.AddTransient<GlobalExceptionHandlingMiddleware>();
        services.AddScoped<GetSubDepartmentServices>();

        // CustomAttributeData services
        services.AddSingleton<IEmailService, EmailService>();
        services.Configure<SMTPParamterSettings>(config.GetSection("EmailSettings"));

        
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
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidAudience = "EManagementAPI",
                ValidIssuer = "EManagementAPI",
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(options =>
        {
            // options.AddPolicy("OrganizationAdmin", policy => policy.RequireRole(["HR", "Admin"]));
            options.AddPolicy("CreateEmployee", policy => policy.RequireRole(["HR", "Admin"]));
            options.AddPolicy("HR", policy => policy.RequireRole("HR"));
            options.AddPolicy("EmployeeHR", policy => policy.RequireRole(["HR", "Employee"]));
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
            options.AddPolicy("DepartmentHead", policy => policy.RequireRole("HeadOfDepartment"));
        });

        services.AddIdentity<ApplicationUser, IdentityRole>(options => {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddApiEndpoints();

        services.AddCors(options => {
            options.AddPolicy("AllowAny", builder => {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddHttpContextAccessor();

        builder.Host.UseSerilog((context, loggerConfig) => {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
        });

        return services;
    }
}