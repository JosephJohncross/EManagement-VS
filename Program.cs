using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning.Builder;
using Carter;
using EManagementVSA.Configuration;
using EManagementVSA.Data;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

FirebaseApp.Create(new AppOptions() {
    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emag-56497-firebase-adminsdk-sb5fr-ef4ae06167.json")),
});

// builder.Services.AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});
builder.Services.AddProjectConfiguration(builder.Configuration, builder);

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
    // setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VV";
    options.SubstituteApiVersionInUrl = true;
});


var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(description.GroupName, new()
        {
            Contact = new()
            {
                Email = "josephibochi2@gmail.com",
                Name = "Joseph Ibochi",
                Url = new Uri("https://linkedin/joseph-ibochi")
            },
            Description = "Through this API, you can manage all authentication and authorization for the whole organization",
            Version = description.ApiVersion.ToString(),
            Title = "Employee Management API",
            License = new()
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });

    }

    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
    foreach (var xmlFile in xmlFiles)
    {
        options.IncludeXmlComments(xmlFile);
    }

    // Add custom operation for health check
    // options.SwaggerDoc("health", new () { Title = "Health Check", Version = "v1" });
    // options.DocumentFilter<HealthDocumentFilter>();
});


var app = builder.Build();

app.UseCors("AllowAny");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setupAction =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            setupAction.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Employee Management API {description.GroupName.ToUpperInvariant()}");
        }
    });
}

using(var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope()) {
    SeedData.Initialize(serviceScope.ServiceProvider);
}

app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();

app.UseSerilogRequestLogging();

// ApiVersionSet apiVersionSet = app.NewApiVersionSet()
//                                         .HasApiVersion(new ApiVersion(1))
//                                         .ReportApiVersions()
//                                         .Build();

// RouteGroupBuilder routeGroupBuilder = app
//     .MapGroup("api/v{v:apiVersion}/")    
//     .WithApiVersionSet(apiVersionSet); 

// routeGroupBuilder            
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();               

app.UseHttpsRedirection();

app.Run();