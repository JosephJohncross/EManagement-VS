namespace EManagementVSA.Features.Department.GetSubDepartment;

public class GetSubDepartmentsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .Build();


        app.MapGet("api/v{v:apiVersion}/department/{id}/departments", async (ISender sender, Guid id) =>
        {
            var getSubDepartmentQuery = new GetSubDepartmentsQuery { departmentId = id };

            return Results.Ok(await sender.Send(getSubDepartmentQuery));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetSubDepartments")
        .WithOpenApi()
        .WithTags("Department")
        .MapToApiVersion(1);
    }
}