using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace EManagementVSA.Features.Organization.ExportDepartments.ExportDepartmentsAsPdf;
public class ExportDepartmentsAsPdfEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app
            .NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .Build();


        app.MapGet("api/v{v:apiVersion}/organization/department/pdf", [Authorize(Policy = "HR", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (ISender _mediator) =>
        {
            var dataDict = await _mediator.Send(new ExportDepartmentAsPdfQuery());

            var renderer = new ChromePdfRenderer();
            using var pdfDocument = renderer.RenderHtmlAsPdf(dataDict["html"]);

            return Results.File(pdfDocument.BinaryData, "application/pdf", $"{dataDict["orgName"]}-Departments.pdf");
        })
        .WithApiVersionSet(apiVersionSet)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        // .Accepts<AddRequest>("application/json")
        .WithName("ExportDepartmentAsPdf")
        .WithOpenApi()
        .WithTags("Export")
        .MapToApiVersion(1);
    }
}