using System;
namespace EManagementVSA.Features.Organization.GetEmployees;

public class GetEmployeesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapGet("api/v{v:apiVersion}/organization/employees", async (ISender sender) => {
            
            return Results.Ok();
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("GetOrganizationEmployees")
        .WithOpenApi()
        .WithTags("Organization")
        .MapToApiVersion(1);
    }
}