namespace EManagementVSA.Features.Department.Update;

public record UpdateDepartmentRequest(
    string? Name,
    string? Abbreviation,
    Guid? ParentDepartmentId
);