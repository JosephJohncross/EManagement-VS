namespace EManagementVSA.Entities;

public class Department
{
    public Guid Id { get; set; }
    public  string Name { get; set; }
    public string Abbreviation { get; set; }
    public List<Department> SubDepartments { get; set; }
    public Department? ParentDepartment { get; set; }
    public Guid? ParentDepartmentId { get; set; }
    public List<Employee> Employees { get; set; } = new List<Employee>();
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; }
}