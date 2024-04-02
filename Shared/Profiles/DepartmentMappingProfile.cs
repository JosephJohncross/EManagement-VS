using EManagementVSA.Entities;
using EManagementVSA.Features.Department.Create;
using EManagementVSA.Features.Department.GetById;

namespace EManagementVSA.Shared.Profiles;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Guid, Department>()
            .ConstructUsing(guid => new Department {Id = guid});

        CreateMap<Department, CreateDepartmentRequest>()
            .ForMember(dest => dest.ParentDepartmentId, opt => opt.MapFrom(src => src.ParentDepartmentId))
            .ReverseMap();

        
        CreateMap<Department, GetByIdResponse>()
            .ForMember(d => d.Organization, opt => opt.MapFrom(s => s.OrganizationId))
            .ReverseMap();
            
    }
}