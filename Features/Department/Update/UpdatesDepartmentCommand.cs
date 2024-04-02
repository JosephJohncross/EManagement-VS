using EManagementVSA.Data;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;

namespace EManagementVSA.Features.Department.Update;

public class UpdatesDepartmentCommand : IRequest<BaseResponse>
{
    public Guid departmentId { get; set; }
    public UpdateDepartmentRequest updateData { get; set; }
}

public class UpdatesDepartmentCommandHandler : IRequestHandler<UpdatesDepartmentCommand, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    public UpdatesDepartmentCommandHandler(ApplicationDbContext context) => _context = context;
    
    public async Task<BaseResponse> Handle(UpdatesDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments.FindAsync(request.departmentId);
        if (department == null) {
            throw new EmployeeManagementNotFoundException("Department does not exist");
        }

        if (request.updateData.Name != null){
            department.Name = request.updateData.Name;
        }

        if (request.updateData.Abbreviation != null){
            department.Abbreviation = request.updateData.Abbreviation;
        }

        if (request.updateData.ParentDepartmentId != null){
            department.ParentDepartmentId = request.updateData.ParentDepartmentId;
        }

        _context.Departments.Update(department);
        await _context.SaveChangesAsync();

        return new BaseResponse {
            Message = "Deparment updated succesfully",
            Status = true
        };
    }
}
