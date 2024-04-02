using EManagementVSA.Features.Department.GetById;
using EManagementVSA.Middlewares.GlobalExceptionHandlingMiddleware;
using EManagementVSA.Shared.Contract;

namespace EManagementVSA.Features.Department.GetSubDepartment
{
    public class GetSubDepartmentsQuery : IRequest<BaseResponse>
    {
        public Guid departmentId { get; set; }
    }

    public class GetSubDepartmentsQueryHandler : IRequestHandler<GetSubDepartmentsQuery, BaseResponse>
    {
        private readonly GetSubDepartmentServices _getSubDepartmentServices;
        private readonly IMapper _mapper;
        public GetSubDepartmentsQueryHandler(GetSubDepartmentServices getSubDepartmentServices, IMapper mapper)
        {
            _getSubDepartmentServices = getSubDepartmentServices;
            _mapper = mapper;
        }
        public async Task<BaseResponse> Handle(GetSubDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var departments = await _getSubDepartmentServices.GetSubDepartments(request.departmentId);

            // var departmentsResponse = new List<GetByIdResponse>();

            // foreach (var dept in departments){
            //     departmentsResponse.Add(DepartmentMappings.MapDepartmentToGetByIdResponse(dept));
            // }

            return new BaseResponse {
                Data = departments,
                Status = true,
                Message = "Operation successful"
            };
        }
    }
}