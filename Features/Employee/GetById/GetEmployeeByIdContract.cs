namespace EManagementVSA.Features.Employee.GetById;

public class GetEmployeeByIdResponse {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public string OrganizationName { get; set; }
    public DateOnly EmploymentDate { get; set; }
}