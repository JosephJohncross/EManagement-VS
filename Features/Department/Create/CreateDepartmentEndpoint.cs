using Microsoft.AspNetCore.Mvc;

namespace EManagementVSA.Features.Department.Create;

public class CreateDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapPost("api/v{v:apiVersion}/department/create_department", async (ISender sender, [FromBody] CreateDepartmentRequest createDeptRequestData) => {
            var createDepartmentCommand = new CreateDepartmentCommand{
                departmentRequestData = createDeptRequestData
            };

            return Results.Ok(await sender.Send(createDepartmentCommand));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("CreateDepartment")
        .WithOpenApi()
        .WithTags("Department")
        .MapToApiVersion(1);
    }
}