using EManagementVSA.Data;
using EManagementVSA.Entities;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Features.Organization.Add;

public class AddOrganizationCommand : IRequest<BaseResponse>
{
    public AddRequest requestData { get; set; }
}

public class AddOrganizationCommandHadler : IRequestHandler<AddOrganizationCommand, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<AddRequest> _validator;
    public AddOrganizationCommandHadler(ApplicationDbContext context, IMapper mapper, IValidator<AddRequest> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }
    public async Task<BaseResponse> Handle(AddOrganizationCommand request, CancellationToken cancellationToken)
    {
        var org = _mapper.Map<Entities.Organization>(request.requestData);
        var organisation = await _context.Organizations.FirstOrDefaultAsync(e => e.Email == org.Email);

        if (organisation != null) {
            throw new EmployeeManagementBadRequestException("Email already exist");
        }
        await _context.Organizations.AddAsync(org, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new BaseResponse {
            Message = "Organisation created successfully"
        };
    }
}
