using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace EManagementVSA.Features.Organization.GetDepartments;

public class GetDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapGet("api/v{v:apiVersion}/organization/{id}/departments", [Authorize(Policy = "CreateEmployee", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (ISender sender, Guid id) => {
            
            var getDepartmentsQuery = new GetDepartmentQuery {OrganizationId = id};
            return Results.Ok(await sender.Send(getDepartmentsQuery));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status403Forbidden)
        .WithName("GetOrganizationDepartments")
        .WithOpenApi()
        .WithTags("Organization")
        .MapToApiVersion(1);
    }
}