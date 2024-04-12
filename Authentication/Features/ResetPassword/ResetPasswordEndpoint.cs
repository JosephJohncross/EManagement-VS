
namespace EManagementVSA.Authentication.Features.ResetPassword;

public class ResetPasswordEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapPost("api/v{v:apiVersion}/reset_password", async (ISender sender) => {
            return Results.Ok();
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("ResetPassword")
        .WithOpenApi()
        .WithTags("Auth")
        .MapToApiVersion(1);
    }
}