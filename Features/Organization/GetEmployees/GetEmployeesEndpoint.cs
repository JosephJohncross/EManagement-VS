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

        
        app.MapGet("api/v{v:apiVersion}/organization/{id}/employees", async (ISender sender, Guid id) => {
            var getEmployeeByOrganization = new GetEmployeesQuery {organizationId = id};
            return Results.Ok(await sender.Send(getEmployeeByOrganization));
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