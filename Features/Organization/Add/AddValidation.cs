namespace EManagementVSA.Features.Organization.Add;

public class AddValidation : AbstractValidator<AddRequest>
{
    public AddValidation()
    {
        RuleFor(org => org.Name).NotEmpty().WithMessage("Name of organization is required");
        RuleFor(org => org.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(org => org.City).NotEmpty().WithMessage("City is required");
        RuleFor(org => org.State).NotEmpty().WithMessage("State is required");
        RuleFor(org => org.Country).NotEmpty().WithMessage("Country is required");
        RuleFor(org => org.PostalCode).NotEmpty().WithMessage("PostalCode is required");
    }
}