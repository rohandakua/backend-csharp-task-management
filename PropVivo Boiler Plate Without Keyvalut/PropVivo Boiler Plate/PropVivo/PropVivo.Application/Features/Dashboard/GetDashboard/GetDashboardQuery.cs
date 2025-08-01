using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Dashboard;

namespace PropVivo.Application.Features.Dashboard.GetDashboard
{
    public class GetDashboardQuery : IRequest<BaseResponse<DashboardResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
} 