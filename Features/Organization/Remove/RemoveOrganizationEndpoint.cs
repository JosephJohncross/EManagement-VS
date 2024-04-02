namespace EManagementVSA.Features.Organization.Remove;

public class RemoveOrganizationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
            ApiVersionSet apiVersionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .Build();

            
            app.MapDelete("api/v{v:apiVersion}/organization/{id}", async (ISender sender, Guid id) => {
                var removeOrganizationCommand = new RemoveOrganizationCommand {OrganizationId = id};

                return Results.Ok(await sender.Send(removeOrganizationCommand));
            })
            .WithApiVersionSet(apiVersionSet)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("RemoveOrganisation")
            .WithOpenApi()
            .WithTags("Organization")
            .MapToApiVersion(1);
    }
}