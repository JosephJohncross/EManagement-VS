namespace EManagementVSA.Entities;

public class Organization
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<Department> Departments { get; set; } = [];
    public List<Employee> Employees { get; set; } = [];
    public Address? Address { get; set; }

}