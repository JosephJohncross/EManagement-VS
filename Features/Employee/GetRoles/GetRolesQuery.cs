using EManagementVSA.Shared.Contract;

namespace EManagementVSA.Features.Employee.GetRoles;

public class GetRolesQuery : IRequest<BaseResponse>
{
    
}

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, BaseResponse>
{
    public async Task<BaseResponse> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        List<string> roles = new() {
            "HR",
            "HeadOfDepartment",
            "Admin"
        };

        return new BaseResponse
        {
            Data = roles,
            Status = true
        };
    }
}
