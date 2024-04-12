namespace EManagementVSA.Authentication.Features.Login;

public record LoginRequestData(
    string Email,
    string Password,
    bool RememberMe = false
);

public class LoginValidator : AbstractValidator<LoginRequestData>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter a valid email address");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password field is required");
    }
}