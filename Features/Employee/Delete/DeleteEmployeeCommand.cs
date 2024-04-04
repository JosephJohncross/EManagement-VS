using EManagementVSA.Data;
using EManagementVSA.Entities;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using Microsoft.AspNetCore.Identity;

namespace EManagementVSA.Features.Employee.Delete;

public class DeleteEmployeeCommand : IRequest<BaseResponse>
{
    public Guid employeeId { get; set; }
}

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, BaseResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public DeleteEmployeeCommandHandler(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _context = context;
    }
    public async Task<BaseResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(request.employeeId);
        if (employee == null)
        {
            throw new EmployeeManagementNotFoundException("Employee does not exist");
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return new BaseResponse
        {
            Message = "Employee deleted succesfully",
            Status = true
        };
    }
}
