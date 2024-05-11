using EManagementVSA.Data;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Features.Department.GetById;

public record GetByIdResponse(
        string Name,
        string Abbreviation,
        Guid DepartmentId
);

public static class DepartmentMappings
{
        public static GetByIdResponse MapDepartmentToGetByIdResponse(Entities.Department department)
        {
                var subDepartmentsDTO = department.SubDepartments?
                        .Select(subDept => MapDepartmentToGetByIdResponse(subDept))
                        .ToList();

                return new GetByIdResponse(
                        department.Name,
                        department.Abbreviation,
                        department.Id
                );
        }
}