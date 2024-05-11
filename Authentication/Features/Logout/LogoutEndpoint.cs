
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;

namespace EManagementVSA.Authentication.Features.Logout;

public class LogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapPost("api/v{v:apiVersion}/logout", async (ISender sender) => {
            var logoutCommand = new LogoutCommand();
            return Results.Ok(await sender.Send(logoutCommand));
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .WithName("Logout")
        .WithOpenApi()
        .WithTags("Auth")
        .MapToApiVersion(1);
    }
}