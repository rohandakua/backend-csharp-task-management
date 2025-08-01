using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Dashboard;

namespace PropVivo.Application.Features.Dashboard.GetSuperiorDashboard
{
    public class GetSuperiorDashboardQuery : IRequest<BaseResponse<DashboardResponse>>
    {
        public string SuperiorId { get; set; } = string.Empty;
    }
} 