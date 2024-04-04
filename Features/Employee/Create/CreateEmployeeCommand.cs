using EManagementVSA.Data;
using EManagementVSA.Entities;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Services.Mail;
using EManagementVSA.Shared.Contract;
using EManagementVSA.Shared.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Features.Employee.Create;

public class CreateEmployeeCommand : IRequest<BaseResponse<string>>
{
    public CreateEmployeeRequest EmployeeeDetails { get; set; }
}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, BaseResponse<string>>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    public CreateEmployeeCommandHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IEmailService emailService)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _emailService = emailService;
    }
    public async Task<BaseResponse<string>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = _mapper.Map<Entities.Employee>(request.EmployeeeDetails);
        var employeeExist = await _userManager.FindByEmailAsync(request.EmployeeeDetails.Email);
        if (employeeExist != null)
        {
            throw new EmployeeManagementBadRequestException("Employee already exist");
        }

        var organization = await _context.Organizations.FindAsync(request.EmployeeeDetails.OrganizationId);
        if (organization == null)
        {
            throw new EmployeeManagementNotFoundException("Organization does not exist");
        }

        var department = await _context.Departments.FindAsync(request.EmployeeeDetails.DepartmentId);
        if (department == null)
        {
            throw new EmployeeManagementNotFoundException("Department does not exist");
        }

        if (department.OrganizationId != organization.Id){
            throw new EmployeeManagementBadRequestException("Department does not exist");
        }
       
        employee.Department = department;
        employee.Organization = organization;


        await _context.Employees.AddAsync(employee);
        try {
            await _context.SaveChangesAsync();
        } catch (Exception e) {
            throw e.InnerException;
        }

        var user = new ApplicationUser
        {
            OrganizationId = employee.OrganizationId,
            EmployeeId = employee.Id,
            Email = request.EmployeeeDetails.Email,
            UserName = request.EmployeeeDetails.Email
        };

        // var initialPassword = CreateEmployeeHelper.GenerateInitialPassword();
       
        var result = await _userManager.CreateAsync(user, "IP8nKaPt2");
        if (!result.Succeeded)
        {
            throw new EmployeeManagementBadRequestException(result.Errors.ToString());
        }

        // Send onboarding mail and reset password
        // var emailTemplate = EmailTemplateLoader.LoadTemplate("EManagementVSA.EmailTemplate.onboard_template.html", initialPassword);

        // var sendNewEmailParmaeter = new SendEmailParameters(
        //     "Welcome on board. Please login to reset your password",
        //     "",
        //     employee.Email,
        //     "Password Reset",
        //     emailTemplate
        // );

        // await _emailService.SendEmailAsync(sendNewEmailParmaeter);

        // Asign Roles to application user
        if (!await _roleManager.RoleExistsAsync("Employee"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Employee"));
        }
        await _userManager.AddToRoleAsync(user, "Employee");

        if (request.EmployeeeDetails.Positions != null){
            foreach (var role in request.EmployeeeDetails.Positions)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        return new BaseResponse<string>{
            Data = user.Id,
            Status = true,
            Message = "Employee successfully created"
        };
    }
}
