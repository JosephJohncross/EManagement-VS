using System.ComponentModel.DataAnnotations;
using EManagementVSA.Shared.Entities;
using Microsoft.AspNetCore.Identity;

namespace EManagementVSA.Entities;

public class Employee
{
        [Key]
        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid OrganizationId { get; set; }
        public required Department Department { get; set; }
        public Organization Organization { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public DateOnly EmploymentDate { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public List<string> Positions { get; set; } = [];
        public Address? Address { get; set; }
        public int? GetAge()
        {
            return DateTime.Today.Year - DateOfBirth.Year;
        }

        public string GetFullName ()
        {
            return FirstName + " " + LastName;
        }
}