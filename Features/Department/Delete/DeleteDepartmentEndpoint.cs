
namespace EManagementVSA.Features.Department.Delete;

public class DeleteDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapDelete("api/v{v:apiVersion}/department/{id}", async (ISender sender, Guid id) => {
            var deleteDeparatmentCommand = new DeleteDepartmentCommand {departmentId = id};

            return Results.Ok(await sender.Send(deleteDeparatmentCommand));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .WithName("DeleteDepartment")
        .WithOpenApi()
        .WithTags("Department")
        .MapToApiVersion(1);
}
}