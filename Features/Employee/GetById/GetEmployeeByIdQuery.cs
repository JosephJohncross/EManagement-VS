using EManagementVSA.Data;
using EManagementVSA.Features.Department.GetById;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;

namespace EManagementVSA.Features.Employee.GetById;

public class GetEmployeeByIdQuery : IRequest<BaseResponse>
{
    public Guid employeeId { get; set; }
}

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetEmployeeByIdQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(request.employeeId);
        if (employee == null)
        {
            throw new EmployeeManagementNotFoundException("Employee does not exist");
        }

        var department = await _context.Departments.FindAsync(employee.DepartmentId);
        var organization = await _context.Organizations.FindAsync(employee.OrganizationId);
        var employeeResponse = _mapper.Map<GetEmployeeByIdResponse>(employee);

        employeeResponse.DepartmentName = department.Name;
        employeeResponse.OrganizationName = organization.Name;

        return new BaseResponse
        {
            Data = employeeResponse,
            Message = "Operation successful",
            Status = true
        };
    }
}
