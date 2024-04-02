namespace EManagementVSA.Features.Department.GetEmployee;

public class GetEmployeeByDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapGet("api/v{v:apiVersion}/department/{id}/employees", async (ISender sender) => {
            
            return Results.Ok();
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("GetEmployeesByDepartmentB")
        .WithOpenApi()
        .WithTags("Department")
        .MapToApiVersion(1);
    }
}