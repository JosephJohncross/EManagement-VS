
namespace EManagementVSA.Features.Organization.GetDepartments;

public class GetDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapGet("api/v{v:apiVersion}/organization/{id}/departments", async (ISender sender, Guid id) => {
            
            var getDepartmentsQuery = new GetDepartmentQuery {OrganizationId = id};
            return Results.Ok(await sender.Send(getDepartmentsQuery));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("GetOrganizationDepartments")
        .WithOpenApi()
        .WithTags("Organization")
        .MapToApiVersion(1);
    }
}