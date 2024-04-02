using EManagementVSA.Data;
using EManagementVSA.Features.Department.GetById;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Features.Department.GetSubDepartment;

public class GetSubDepartmentServices
{
    private readonly ApplicationDbContext _context;
    public GetSubDepartmentServices(ApplicationDbContext context) => _context = context;

    public async Task<List<GetByIdResponse>> GetSubDepartments(Guid departmentId)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == departmentId);

        if (department == null)
        {
            // Department not found, return an empty list or throw an exception
            throw new EmployeeManagementNotFoundException("Department does not exist");
        }

        var departmentResponse = new List<GetByIdResponse>();
        var subDepartments = await _context.Departments.Where(d => d.ParentDepartmentId == departmentId).ToListAsync();
        foreach (var subDepartment in subDepartments)
        {
            departmentResponse.Add(DepartmentMappings.MapDepartmentToGetByIdResponse(subDepartment));
        }

        return departmentResponse;
    }
}