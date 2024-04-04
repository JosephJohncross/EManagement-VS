using System.ComponentModel.DataAnnotations;

namespace EManagementVSA.Entities;

public class Address
{
    [Key]
    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    
    [MaxLength(5)]
    [MinLength(5)]
    public int PostalCode { get; set; }

    public Guid? OrganizationId { get; set; }
    public Organization Organization { get; set; }

    public Guid? EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public string GetAddress () {
        return  $"{Street}, {City}, {State}, {Country}";
    }
}