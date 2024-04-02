using System.ComponentModel.DataAnnotations;
using EManagementVSA.Shared.Entities;

namespace EManagementVSA.Entities;

public class Organization : BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string Email { get; set; }
    public List<Department> Departments { get; set; } = [];
    public List<Employee> Employees { get; set; } = [];
    public Address? Address { get; set; }
}