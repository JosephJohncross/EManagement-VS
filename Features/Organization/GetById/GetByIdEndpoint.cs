namespace EManagementVSA.Features.Organization.GetById;

public class GetByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapGet("api/v{v:apiVersion}/organization/{id}", async (ISender sender, Guid id) => {

            var getByIdQuery = new GetByIdQuery{
                OrganizationId = id
            };
            
            return Results.Ok(await sender.Send(getByIdQuery));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("GetOrganizationById")
        .WithOpenApi()
        .WithTags("Organization")
        .MapToApiVersion(1)
        .RequireAuthorization("CreateEmployee");
    }
}