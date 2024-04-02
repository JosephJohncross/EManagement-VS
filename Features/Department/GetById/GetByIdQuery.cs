using EManagementVSA.Data;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;

namespace EManagementVSA.Features.Department.GetById;

public class GetByIdQuery : IRequest<BaseResponse>
{
    public Guid departmentId { get; set; }
}

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;
    public GetByIdQueryHandler(ApplicationDbContext context, IMapper mapper, Serilog.ILogger logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var department = await _context.Departments.FindAsync(request.departmentId);

        if (department == null) {
            throw new EmployeeManagementNotFoundException("Department does not exist");
        }

        return new BaseResponse
        {
            Data = DepartmentMappings.MapDepartmentToGetByIdResponse(department),
            Message = "Operation successful",
            Status = true
        };
    }
}
