
using Microsoft.AspNetCore.Mvc;

namespace EManagementVSA.Features.Employee.RemoveRole;

public class RemoveEmployeeRoleEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();
        
        app.MapPatch("api/v{v:apiVersion}/employee/{id}/remove_role", async (ISender sender, [FromBody] string[] Positions, Guid id) => {
            var removeEmpRolesCommand = new RemoveEmployeeRoleCommand {Positions = Positions};
            return Results.Ok(await sender.Send(removeEmpRolesCommand));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .WithName("RemoveemployeeRoles")
        .WithOpenApi()
        .WithTags("Employee")
        .MapToApiVersion(1);
    }
}