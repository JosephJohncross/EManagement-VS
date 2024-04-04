namespace EManagementVSA.Features.Employee.Delete;

public class DeleteEmployeeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapDelete("api/v{v:apiVersion}/employee/{id}", async (ISender sender, Guid id) => {
            var deleteEmployeeCommand = new DeleteEmployeeCommand {employeeId = id};
            return Results.Ok(await sender.Send(deleteEmployeeCommand));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("DeleteEmployee")
        .WithOpenApi()
        .WithTags("Employee")
        .MapToApiVersion(1);
    }
}