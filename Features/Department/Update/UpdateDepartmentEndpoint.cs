namespace EManagementVSA.Features.Department.Update;

public class UpdateDepartmentEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();


        app.MapPatch("api/v{v:apiVersion}/department/{id}", async (ISender sender, UpdateDepartmentRequest updateData, Guid id) =>
        {
            var updateDepartmentCommand = new UpdatesDepartmentCommand 
                { departmentId = id, updateData = updateData };

            return Results.Ok(await sender.Send(updateDepartmentCommand));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("UpdateDepartment")
        .WithOpenApi()
        .WithTags("Department")
        .MapToApiVersion(1);
    }
}