using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EManagementVSA.Data;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;

namespace EManagementVSA.Features.Organization.Remove
{
    public class RemoveOrganizationCommand : IRequest<BaseResponse>
    {
        public Guid OrganizationId { get; set; }
    }

    public class RemoveOrganizationCommandHandler : IRequestHandler<RemoveOrganizationCommand, BaseResponse>
    {
        private readonly ApplicationDbContext _context;
        public RemoveOrganizationCommandHandler(ApplicationDbContext context) => _context = context;
        
        public async Task<BaseResponse> Handle(RemoveOrganizationCommand request, CancellationToken cancellationToken)
        {
            var org = await _context.Organizations.FindAsync(request.OrganizationId);
            if (org == null) {
                throw new EmployeeManagementNotFoundException("Organization does not exist");
            }

            _context.Organizations.Remove(org);
            await _context.SaveChangesAsync(cancellationToken);

            return new BaseResponse {
                Message = "Organization removed successfully",
                Status = true
            };
        }
    }
}