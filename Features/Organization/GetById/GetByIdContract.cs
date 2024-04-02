namespace EManagementVSA.Features.Organization.GetById;

public record GetByIdResponse(
    string Name,
    string Email,
    string Street,
    string City,
    string State,
    string Country,
    int PostalCode
);