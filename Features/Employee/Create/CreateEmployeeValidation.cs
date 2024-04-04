namespace EManagementVSA.Features.Employee.Create;

public class CreateEmployeeValidation : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeValidation()
    {
          RuleFor(org => org.FirstName).NotEmpty().WithMessage("FirstName is required");
          RuleFor(org => org.LastName).NotEmpty().WithMessage("LastName is required");
          RuleFor(org => org.DateOfBirth).NotEmpty().WithMessage("Date of Birth is required");
          RuleFor(org => org.Email).NotEmpty().WithMessage("Email is required");
          RuleFor(org => org.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required");
          RuleFor(org => org.EmploymentDate).NotEmpty().WithMessage("Employement Date is required");
    }
}