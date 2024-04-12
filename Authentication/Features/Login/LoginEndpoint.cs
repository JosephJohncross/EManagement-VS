using EManagementVSA.Shared.Contract;
using Microsoft.AspNetCore.Mvc;

namespace EManagementVSA.Authentication.Features.Login;

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         ApiVersionSet apiVersionSet = app
        .NewApiVersionSet()
        .HasApiVersion(new ApiVersion(1))
        .Build();

        
        app.MapPost("api/v{v:apiVersion}/login", async (ISender sender, IValidator<LoginRequestData> validator, [FromBody] LoginRequestData requestData) => {

            var validationResult = await validator.ValidateAsync(requestData);
            if (validationResult.IsValid){
                var loginCommand = new LoginCommand {requestData = requestData};
                return Results.Ok(await sender.Send(loginCommand));
            } else {

                return Results.BadRequest(new BaseResponse {
                    ValidationErrors = validationResult.Errors,
                    Status = false,
                    Message = "Operation unsuccessful"
                });
            }
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("Login")
        .WithOpenApi()
        .WithTags("Auth")
        .MapToApiVersion(1);
    }
}