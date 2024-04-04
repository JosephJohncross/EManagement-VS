
namespace EManagementVSA.Features.Employee.GetById;

public class GetEmployeeByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapGet("api/v{v:apiVersion}/employee/{id}", async (ISender sender, Guid id) => {
            var getEmployeeById = new GetEmployeeByIdQuery {employeeId = id};
            return Results.Ok(await sender.Send(getEmployeeById));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("GetEmployee")
        .WithOpenApi()
        .WithTags("Employee")
        .MapToApiVersion(1);
    }
}