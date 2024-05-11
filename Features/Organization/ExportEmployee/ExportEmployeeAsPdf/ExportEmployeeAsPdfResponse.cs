namespace EManagementVSA.Features.Organization.ExportEmployee.ExportEmployeeAsPdf;

public record ExportEmployeeAsPdfResponse(
    PdfDocument pdf,
    string organizationName
);