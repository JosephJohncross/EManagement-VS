namespace EManagementVSA.Features.Department.Create;

public record CreateDepartmentRequest(
    string Name,
    string Abbreviation,
    Guid? ParentDepartmentId,
    Guid OrganizationId 
);