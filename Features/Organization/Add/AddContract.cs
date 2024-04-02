namespace EManagementVSA.Features.Organization.Add;

public record AddRequest(
    string? Name,
    string? Email,
    string? Street,
    string? City,
    string? State,
    string? Country,
    int PostalCode
);