using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EManagementVSA.Shared.Entities;

namespace EManagementVSA.Entities;

public class Department : BaseEntity
{
    [Key]
    public Guid Id { get; set; } 
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; }
    public List<Department> SubDepartments { get; set; } = new List<Department>();
     [JsonIgnore]
    public Department? ParentDepartment { get; set; }
    public Guid? ParentDepartmentId { get; set; }
    public List<Employee> Employees { get; set; } = new List<Employee>();
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; }
}