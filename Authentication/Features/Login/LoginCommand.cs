using EManagementVSA.Authentication.Helper;
using EManagementVSA.Entities;
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
    private readonly IConfiguration _config;
    public LoginCommandHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
    {
        _signinManager = signInManager;
        _userManager = userManager;
        _config = config;
    }

    public async Task<BaseResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.requestData.Email);
        if (user == null)
        {
            throw new EmployeeManagementNotFoundException("Email or password incorrect");
        }

        
        var result = await _signinManager.PasswordSignInAsync(user, request.requestData.Password, request.requestData.RememberMe, true);
        if (result.Succeeded){
            var claims = await _userManager.GetClaimsAsync(user);
            var token = GenerateUserToken.CreateToken(claims, _config);

            return new BaseResponse {
                Data = token,
                Message = "Login successful",
                Status = true
            };
        }
            throw new EmployeeManagementBadRequestException("Invalid login credential");
    }
}
