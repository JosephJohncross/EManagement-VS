using Microsoft.AspNetCore.Identity;

namespace EManagementVSA.Entities;

public class ApplicationUser : IdentityUser
{
    public Guid EmployeeId { get; set; }
    public Guid OrganizationId { get; set; }
}