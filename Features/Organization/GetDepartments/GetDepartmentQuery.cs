using System.Security.Claims;
using EManagementVSA.Data;
using EManagementVSA.Entities;
using EManagementVSA.Features.Department.GetById;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Features.Organization.GetDepartments;

public class GetDepartmentQuery : IRequest<BaseResponse>
{
    public Guid OrganizationId { get; set; }
}

public class GetDepartmentQueryHandler : IRequestHandler<GetDepartmentQuery, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    public GetDepartmentQueryHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<BaseResponse> Handle(GetDepartmentQuery request, CancellationToken cancellationToken)
    {
        var employeeClaim = _httpContextAccessor.HttpContext.User;
        
        var user = await _userManager.GetUserAsync(employeeClaim);
        if (user == null) {
            throw new EmployeeManagementForbiddenException("User does not exist");
        }


        var organzation = await _context.Organizations.FindAsync(request.OrganizationId);
        if (organzation == null)
        {
            throw new EmployeeManagementNotFoundException("Department does not exist");
        }
        
        if (organzation.Id != user.OrganizationId) {
            throw new EmployeeManagementForbiddenException("Unauthorized access");
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
