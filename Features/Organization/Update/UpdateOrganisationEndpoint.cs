
using EManagementVSA.Features.Organization.Add;
using Microsoft.AspNetCore.Mvc;

namespace EManagementVSA.Features.Organization.Update;

public class UpdateOrganisationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapPatch("api/v{v:apiVersion}/organization/{id}", async (ISender sender, [FromBody] AddRequest requestData, Guid id) => {
            var updateOrganizationCommand = new UpdateOrganisationCommand {
                organizationId = id,
                requestData = requestData
            };

            return Results.Ok(await sender.Send(updateOrganizationCommand));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("UpdateOrganization")
        .WithOpenApi()
        .WithTags("Organization")
        .MapToApiVersion(1);
    }
}