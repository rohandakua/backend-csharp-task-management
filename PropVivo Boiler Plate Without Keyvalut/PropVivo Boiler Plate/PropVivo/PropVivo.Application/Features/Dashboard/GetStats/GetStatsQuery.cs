using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Dashboard;

namespace PropVivo.Application.Features.Dashboard.GetStats
{
    public class GetStatsQuery : IRequest<BaseResponse<DashboardStats>>
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
} 