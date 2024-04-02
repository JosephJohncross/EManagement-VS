using EManagementVSA.Entities;
using EManagementVSA.Features.Employee.Create;

namespace EManagementVSA.Shared.Profiles;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
         CreateMap<CreateEmployeeRequest, Employee>()
            .ForMember(dto => dto.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dto => dto.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dto => dto.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dto => dto.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dto => dto.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dto => dto.Department, opt => opt.Ignore())
            .ForPath(dto => dto.Department.Id, opt => opt.MapFrom(src => src.DepartmentId))
            .ForPath(dto => dto.Organization.Id, opt => opt.MapFrom(src => src.OrganizationId))
            .ForMember(dto => dto.Positions, opt => opt.MapFrom(src => src.Positions))
            .ReverseMap();
    }
}