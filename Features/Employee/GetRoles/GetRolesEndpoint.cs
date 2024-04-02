
namespace EManagementVSA.Features.Employee.GetRoles;

public class GetRolesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();
        
        app.MapGet("api/v{v:apiVersion}/employee/roles", async (ISender sender) => {
            
            return Results.Ok(await sender.Send(new GetRolesQuery()));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .WithName("GetEmployeeRoles")
        .WithOpenApi()
        .WithTags("Employee")
        .MapToApiVersion(1);
    }
}