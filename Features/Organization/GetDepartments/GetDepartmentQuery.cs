using EManagementVSA.Data;
using EManagementVSA.Features.Department.GetById;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Features.Organization.GetDepartments;

public class GetDepartmentQuery : IRequest<BaseResponse>
{
    public Guid OrganizationId { get; set; }
}

public class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    public GetDepartmentQueryHandler(ApplicationDbContext context) => _context = context;
    public async Task<BaseResponse> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
    {
        var organzation = await _context.Organizations.FindAsync(request.OrganizationId);
        if (organzation == null)
        {
            throw new EmployeeManagementNotFoundException("Department does not exist");
        }

        var departments = await _context.Departments
            .Where(x => x.OrganizationId == organzation.Id)
            .ToListAsync();

        var departmentResponse = new List<GetByIdResponse>();
        foreach (var dept in departments)
        {
            departmentResponse.Add(DepartmentMappings.MapDepartmentToGetByIdResponse(dept));
        }

        return new BaseResponse
        {
            Data = departmentResponse,
            Status = true
        };
    }
}
