
namespace EManagementVSA.Features.Employee.ChangeDepartment;

public class ChangeDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapPatch("api/v{v:apiVersion}/employee/{id}/change_department", async (ISender sender, Guid id) => {
            
            return Results.Ok();
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("ChangeDepartment")
        .WithOpenApi()
        .WithTags("Employee")
        .MapToApiVersion(1);
    }
}