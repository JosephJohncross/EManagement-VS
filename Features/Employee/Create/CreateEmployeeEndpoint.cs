using Microsoft.AspNetCore.Mvc;

namespace EManagementVSA.Features.Employee.Create;

public class CreateEmployeeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        
         ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapPost("api/v{v:apiVersion}/employee/create_employee", async (ISender sender, [FromBody] CreateEmployeeRequest createEmployeeRequest) => {
            var createEmployeeCommand = new CreateEmployeeCommand {EmployeeeDetails = createEmployeeRequest};
            return Results.Ok(await sender.Send(createEmployeeCommand));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("CreateEmployee")
        .WithOpenApi()
        .WithTags("Employee")
        .MapToApiVersion(1);
    }
}