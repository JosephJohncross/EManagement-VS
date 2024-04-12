namespace EManagementVSA.Authentication.Features.RequestPasswordReset;

public class RequestPasswordResetEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapPost("api/v{v:apiVersion}/request_password_reset", async (ISender sender) => {
            return Results.Ok();
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("RequestPasswordReset")
        .WithOpenApi()
        .WithTags("Auth")
        .MapToApiVersion(1);
    }
}