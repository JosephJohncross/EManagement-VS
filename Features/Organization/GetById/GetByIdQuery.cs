using EManagementVSA.Data;
using EManagementVSA.Features.Organization.Add;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Features.Organization.GetById;

public class GetByIdQuery : IRequest<BaseResponse>
{
    public Guid OrganizationId { get; set; }
}

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    public GetByIdQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<BaseResponse> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var organisation = await _context.Organizations.FindAsync(request.OrganizationId);
        if (organisation == null)
        {
            throw new EmployeeManagementNotFoundException("Organization deos not exist");
        }

        var address =  await _context.Address.FirstOrDefaultAsync(x => x.OrganizationId == organisation.Id);

        var organizationData = new GetByIdResponse(
            organisation.Name,
            organisation.Email,
            address.Street,
            address.City,
            address.State,
            address.Country,
            address.PostalCode
        );
     

        return new BaseResponse
        {
            Data = organizationData,
            Status = true,
            Message = "Operation successful"
        };
    }
}
