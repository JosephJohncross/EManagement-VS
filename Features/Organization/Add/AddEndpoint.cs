using EManagementVSA.Shared.Contract;
using Microsoft.AspNetCore.Mvc;

namespace EManagementVSA.Features.Organization.Add;

public class AddEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .Build();


        app.MapPost("api/v{v:apiVersion}/create_organization", async (ISender _mediator, IValidator<AddRequest> _validator, [FromBody] AddRequest requestData) => {

             var validationResult = _validator.Validate(requestData);
            if (!validationResult.IsValid) {
                return Results.BadRequest(new BaseResponse {
                    ValidationErrors = validationResult.Errors,
                    Status = false,
                    Message = "Operation not successful"
                });
            }

            AddOrganizationCommand addOrganizationCommand = new () {requestData = requestData};
            var response = await _mediator.Send(addOrganizationCommand);

            return Results.Ok(response);
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Accepts<AddRequest>("application/json")
        .WithName("CreateOrganization")
        .WithOpenApi()
        .WithTags("Organization")
        .MapToApiVersion(1)
        .RequireAuthorization("Admin");
    }
}