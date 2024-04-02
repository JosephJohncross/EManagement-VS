namespace EManagementVSA.Features.Employee.Create;

public record CreateEmployeeRequest(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber,
    Guid DepartmentId,
    Guid OrganizationId,
    List<string> Positions
);
