namespace EManagementVSA.Features.Employee.Create;

public record CreateEmployeeRequest(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber,
    Guid DepartmentId,
    Guid OrganizationId,
    List<string>? Positions,
    DateOnly EmploymentDate,
    Role Role
);

public enum Role {
    HR,
    DepartmentHead,
    Employee
}

public static class CreateEmployeeContract {
    public static string RoleReprentation (Role role) {

        if ( role is Role.HR ) {
            return "HR";
        }else if (role is Role.DepartmentHead){
            return "DepartmentHead";
        }else if (role is Role.Employee){
            return "Employee";
        }
        return "";
    }

} 