using System.Security.Claims;
using EManagementVSA.Authentication.Helper;
using EManagementVSA.Data;
using EManagementVSA.Entities;
using EManagementVSA.Features.Department.GetById;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using Microsoft.AspNetCore.Identity;

namespace EManagementVSA.Authentication.Features.Login;

public class LoginCommand : IRequest<BaseResponse>
{
    public LoginRequestData requestData { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signinManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    public LoginCommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config, ApplicationDbContext context)
    {
        _signinManager = signInManager;
        _userManager = userManager;
        _config = config;
        _context = context;
    }

    public async Task<BaseResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.requestData.Email);
        if (user == null)
        {
            throw new EmployeeManagementNotFoundException("Invalid login credential");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var isHr = await _userManager.IsInRoleAsync(user, "HR");

        var claims = new List<Claim>();
        
        if (roles != null) {
            foreach (var role in roles) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            // claims.Add(new Claim("OrganizationID", user.OrganizationId.ToString()));
        }
        
     
        var token = GenerateUserToken.CreateToken(_config,  claims);
        if (isHr){
            var organization = await _context.Organizations.FindAsync(user.OrganizationId);
            var organizationDetails =  new OrganizationDetails(
                organization.Id,
                organization.Name,
                organization.Email
            );
            var loginResponse = new LoginResponseData(token, (List<string>) roles);

            return new BaseResponse {
                Data = new LoginResponseDataWithOrgDetails(loginResponse, organizationDetails),
                Message = "Login successful",
                Status = true
            };

        } else {
            return new BaseResponse {
                Data = new LoginResponseData(token, (List<string>) roles),
                Message = "Login successful",
                Status = true
            };
        }

    }
}
