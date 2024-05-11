namespace EManagementVSA.Features.Organization.GetEmployees;

public class GetEmployeeByOrganizationResponse {
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly EmploymentDate { get; set; }
    public string DepartmentName { get; set; }
    public List<string> Positions { get; set; }
}

public static class EmployeeMappings
{
        public static GetEmployeeByOrganizationResponse MapEmployeeToGetEmployeeResponse(Entities.Employee employee, string departmentName)
        {

            return new GetEmployeeByOrganizationResponse {
                DepartmentName = departmentName,
                Email = employee.Email,
                EmploymentDate = employee.EmploymentDate,
                FullName = employee.GetFullName(),
                Id = employee.Id,
                PhoneNumber = employee.PhoneNumber,
                Positions = employee.Positions,
                DateOfBirth = employee.DateOfBirth,
            };
        }
}