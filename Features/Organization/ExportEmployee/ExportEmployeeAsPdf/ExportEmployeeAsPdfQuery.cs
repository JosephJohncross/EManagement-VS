using EManagementVSA.Data;
using EManagementVSA.Entities;
using EManagementVSA.Features.Organization.GetEmployees;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Razor.Templating.Core;

namespace EManagementVSA.Features.Organization.ExportEmployee.ExportEmployeeAsPdf;

public class ExportEmployeeAsPdfQuery : IRequest<Dictionary<string, string>>
{

}


public class ExportEmployeeAsPdfQueryHandler : IRequestHandler<ExportEmployeeAsPdfQuery, Dictionary<string, string>>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    public ExportEmployeeAsPdfQueryHandler(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _context = context;
    }

    public async Task<Dictionary<string, string>> Handle(ExportEmployeeAsPdfQuery request, CancellationToken cancellationToken)
    {
         var employeeClaim = _contextAccessor.HttpContext.User;
            
            var user = await _userManager.GetUserAsync(employeeClaim);
            if (user == null) {
                throw new EmployeeManagementForbiddenException("No permission granted");
            }
            
            var organization = await _context.Organizations.FindAsync(user.OrganizationId);

            var employeeResponseList = new List<GetEmployeeByOrganizationResponse>();
            var employees = await _context.Employees.Where(x => x.OrganizationId == user.OrganizationId).ToListAsync();

            if (employees.Count > 0) {
                foreach (var empl in employees){
                    var department = await _context.Departments.FindAsync(empl.DepartmentId);
                    employeeResponseList.Add(EmployeeMappings.MapEmployeeToGetEmployeeResponse(empl, department.Name));
                }
            }

            var html = await RazorTemplateEngine.RenderAsync("Infrastructure/FileGeneration/Pdf/Views/EmployeePdf.cshtml", employeeResponseList);

            var dataDict = new Dictionary<string, string>
            {
                { "html", html },
                { "orgName", organization.Name }
            };

            return dataDict;

    }
}
