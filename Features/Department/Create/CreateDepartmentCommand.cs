using EManagementVSA.Data;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Features.Department.Create;

public class CreateDepartmentCommand : IRequest<BaseResponse>
{
    public CreateDepartmentRequest departmentRequestData { get; set; }
}

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public CreateDepartmentCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = _mapper.Map<Entities.Department>(request.departmentRequestData);

        var departmentExist = await _context.Departments
            .AnyAsync(x => 
            (x.Name == department.Name || x.Abbreviation.ToLower() == department.Abbreviation.ToLower()) &&
            x.OrganizationId == department.OrganizationId);

        if (departmentExist)
        {
            throw new EmployeeManagementBadRequestException("Department already exist or abbreviation has been taken");
        }

        var organizationExist = await _context.Organizations.FindAsync(request.departmentRequestData.OrganizationId);
        if (organizationExist == null)
        {
            throw new EmployeeManagementNotFoundException("Organization does not exist");
        }

        if (department.ParentDepartmentId != null){
            var parentDepartmentExist = await _context.Departments.FindAsync(request.departmentRequestData.ParentDepartmentId);
            //Checks if the department exist in the organisation 
            if (parentDepartmentExist == null || parentDepartmentExist.OrganizationId != department.OrganizationId)
            {
                throw new EmployeeManagementNotFoundException("Department does not exist");
            }
        }


        await _context.Departments.AddAsync(department);
        await _context.SaveChangesAsync();

        return new BaseResponse {
            Data = department.Id,
            Status = true,
            Message = "Department successfully created"
        };
    }
}
