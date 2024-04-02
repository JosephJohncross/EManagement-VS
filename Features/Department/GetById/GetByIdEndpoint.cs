using EManagementVSA.Features.Department.GetById;

namespace EManagementVSA.Features.Department.GetByIdEndpoint
{
    public class GetById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .Build();

            
            app.MapGet("api/v{v:apiVersion}/department/{id}", async (ISender sender, Guid id) => {
                var getByIdQuery = new GetByIdQuery {departmentId = id};

                return Results.Ok(await sender.Send(getByIdQuery));
            })
            .WithApiVersionSet(apiVersionSet)
            .Produces(StatusCodes.Status201Created)
            .WithName("GetDepartmentById")
            .WithOpenApi()
            .WithTags("Department")
            .MapToApiVersion(1);
        }
    }
}