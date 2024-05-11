using EManagementVSA.Entities;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using Microsoft.AspNetCore.Identity;

namespace EManagementVSA.Authentication.Features.Logout;

public class LogoutCommand : IRequest<BaseResponse>
{
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, BaseResponse>
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    public LogoutCommandHandler(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    public async Task<BaseResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
        return new BaseResponse{
            Message = "Successfully signout",
            Status = true
        };
    }
}
