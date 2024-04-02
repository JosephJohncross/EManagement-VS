using EManagementVSA.Entities;
using EManagementVSA.Features.Department.GetById;
using EManagementVSA.Features.Organization.Add;

namespace EManagementVSA.Shared.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Organization, AddRequest>()
            .ForMember(o => o.Email, a => a.MapFrom(src => src.Email))
            .ForMember(o => o.Name, a => a.MapFrom(src => src.Name))
            .ForMember(o => o.Street, a => a.MapFrom(src => src.Address.Street))
            .ForMember(o => o.City, a => a.MapFrom(src => src.Address.City))
            .ForMember(o => o.State, a => a.MapFrom(src => src.Address.State))
            .ForMember(o => o.Country, a => a.MapFrom(src => src.Address.Country))
            .ForMember(o => o.PostalCode, a => a.MapFrom(src => src.Address.PostalCode))
            .ReverseMap();
    }
}