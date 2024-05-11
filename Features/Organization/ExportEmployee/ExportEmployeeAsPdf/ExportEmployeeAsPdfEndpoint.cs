using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EManagementVSA.Features.Organization.ExportEmployee.ExportEmployeeAsPdf;

public class ExportEmployeeAsPdfEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .Build();


        app.MapGet("api/v{v:apiVersion}/organization/employees/pdf", [Authorize(Policy = "HR", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (ISender _mediator) =>
        {
            var dataDict = await _mediator.Send(new ExportEmployeeAsPdfQuery());

            var renderer = new ChromePdfRenderer();
            using var pdfDocument = renderer.RenderHtmlAsPdf(dataDict["html"]);

            return Results.File(pdfDocument.BinaryData, "application/pdf", $"{dataDict["orgName"]}-Employees.pdf");
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        // .Accepts<AddRequest>("application/json")
        .WithName("ExportEmployeeAsPdf")
        .WithOpenApi()
        .WithTags("Export")
        .MapToApiVersion(1);
    }
}