using System;
using EManagementVSA.Data;
using EManagementVSA.Entities;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using Microsoft.AspNetCore.Identity;

namespace EManagementVSA.Features.Employee.RemoveRole;

public class RemoveEmployeeRoleCommand : IRequest<BaseResponse>
{
    public Guid employeeId { get; set; }
    public string[] Positions { get; set; }
}

public class RemoveEmployeeRoleCommandHandler : IRequestHandler<RemoveEmployeeRoleCommand, BaseResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    public RemoveEmployeeRoleCommandHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<BaseResponse> Handle(RemoveEmployeeRoleCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(request.employeeId);
        if (employee == null){
            throw new EmployeeManagementNotFoundException("EMployee does not exist");
        }

        var user = await _userManager.FindByEmailAsync(employee.Email);

        foreach (var position in request.Positions){
            if (await _userManager.IsInRoleAsync(user, position)){
                await _userManager.RemoveFromRoleAsync(user, position);
            } else {
                throw new EmployeeManagementNotFoundException($"No position exist for current employee");
            }
        }

        return new BaseResponse {
            Status = true,
            Message = "Operation successful"
        };

    }
}
