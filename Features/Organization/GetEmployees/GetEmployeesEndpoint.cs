using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
namespace EManagementVSA.Features.Organization.GetEmployees;

public class GetEmployeesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapGet("api/v{v:apiVersion}/organization/{id}/employees", [Authorize(Policy = "CreateEmployee", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (ISender sender, Guid id) => {
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
        // .RequireAuthorization("CreateEmployee");
    }
}