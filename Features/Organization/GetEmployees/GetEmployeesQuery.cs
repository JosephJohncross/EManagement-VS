using EManagementVSA.Data;
using EManagementVSA.Shared.Contract;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Features.Organization.GetEmployees;

public class GetEmployeesQuery : IRequest<BaseResponse>
{
    public Guid organizationId { get; set; }
}

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    public GetEmployeesQueryHandler(ApplicationDbContext context) => _context = context;

    public async Task<BaseResponse> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employeeResponseList = new List<GetEmployeeByOrganizationResponse>();
        var employees = await _context.Employees
            .Where(x => x.OrganizationId == request.organizationId)
            .ToListAsync();

        if (employees.Count > 0) {
            foreach (var empl in employees){
                var department = await _context.Departments.FindAsync(empl.DepartmentId);
                employeeResponseList.Add(EmployeeMappings.MapEmployeeToGetEmployeeResponse(empl, department.Name));
            }
        }
        

        return new BaseResponse
        {
            Data = employeeResponseList,
            Message = "Operation successful",
            Status = true
        };
    }
}
