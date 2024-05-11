
using EManagementVSA.Data;
using EManagementVSA.Entities;
using EManagementVSA.Features.Department.GetById;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Razor.Templating.Core;

namespace EManagementVSA.Features.Organization.ExportDepartments.ExportDepartmentsAsPdf;

public class ExportDepartmentAsPdfQuery : IRequest<Dictionary<string, string>>
{}

public class ExportDepartmentAsPdfQueryHandler : IRequestHandler<ExportDepartmentAsPdfQuery, Dictionary<string, string>>
{
     private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    public ExportDepartmentAsPdfQueryHandler(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _context = context;
    }   
    public async Task<Dictionary<string, string>> Handle(ExportDepartmentAsPdfQuery request, CancellationToken cancellationToken)
    {
        var employeeClaim = _contextAccessor.HttpContext.User;
         
        var user = await _userManager.GetUserAsync(employeeClaim);
        if (user == null) {
            throw new EmployeeManagementForbiddenException("No permission granted");
        }

        var organization = await _context.Organizations.FindAsync(user.OrganizationId);
        
        var departments = await _context.Departments
            .Where(x => x.OrganizationId == organization.Id)
            .ToListAsync();

        var departmentResponse = new List<GetByIdResponse>();
        foreach (var dept in departments)
        {
            departmentResponse.Add(DepartmentMappings.MapDepartmentToGetByIdResponse(dept));
        }

        var html = await RazorTemplateEngine.RenderAsync("Infrastructure/FileGeneration/Pdf/Views/DepartmentsPdf.cshtml", departmentResponse);

        var dataDict = new Dictionary<string, string>
        {
            { "html", html },
            { "orgName", organization.Name }
        };

        return dataDict;
    }
}
