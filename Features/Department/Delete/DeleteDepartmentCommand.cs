using EManagementVSA.Data;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;

namespace EManagementVSA.Features.Department.Delete;

public class DeleteDepartmentCommand : IRequest<BaseResponse>
{
    public Guid departmentId { get; set; }
}

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    public DeleteDepartmentCommandHandler(ApplicationDbContext context) => _context = context;

    public async Task<BaseResponse> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments.FindAsync(request.departmentId);
        if (department == null) {
            throw new EmployeeManagementNotFoundException("Department does not exist");
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();

        return new BaseResponse {
            Message = "Department deleted successfully",
            Status = true
        };
    }
}
