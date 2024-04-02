using EManagementVSA.Data;
using EManagementVSA.Entities;
using EManagementVSA.Features.Organization.Add;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Features.Organization.Update;

public class UpdateOrganisationCommand : IRequest<BaseResponse>
{
    public Guid organizationId { get; set; }
    public AddRequest requestData { get; set; }
}

public class UpdateOrganisationCommandHandler : IRequestHandler<UpdateOrganisationCommand, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateOrganisationCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BaseResponse> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
    {
        var organization = await _context.Organizations.FindAsync(request.organizationId);
        if (organization == null)
        {
            throw new EmployeeManagementNotFoundException("Organization does not exist");
        }

        // Update organization properties if they are not null in the request
        if (request.requestData.Name != null)
        {
            organization.Name = request.requestData.Name;
        }

        if (request.requestData.Email != null)
        {
            organization.Email = request.requestData.Email;
        }


        var address = await _context.Address.FirstOrDefaultAsync(x => x.OrganizationId == organization.Id);
        if (address == null)
        {
            throw new EmployeeManagementNotFoundException("Address not found for this organization");
        }

        if (request.requestData.Street != null)
        {
            address.Street = request.requestData.Street;
        }

        if (request.requestData.City != null)
        {
            address.City = request.requestData.City;
        }

        if (request.requestData.State != null)
        {
            address.State = request.requestData.State;
        }

        if (request.requestData.Country != null)
        {
            address.Country = request.requestData.Country;
        }

        if (request.requestData.PostalCode != 0) // Assuming 0 is not a valid PostalCode
        {
            address.PostalCode = request.requestData.PostalCode;
        }

        // Update address in the context
        _context.Address.Update(address);

        // Update organization in the context
        _context.Organizations.Update(organization);
        await _context.SaveChangesAsync();

        return new BaseResponse
        {
            Message = "Organisation updated successfully",
            Status = true
        };
    }
}
