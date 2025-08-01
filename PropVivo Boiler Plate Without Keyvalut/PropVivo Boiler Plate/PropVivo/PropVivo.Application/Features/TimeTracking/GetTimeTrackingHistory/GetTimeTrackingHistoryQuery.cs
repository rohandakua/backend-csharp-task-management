using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TimeTracking;

namespace PropVivo.Application.Features.TimeTracking.GetTimeTrackingHistory
{
    public class GetTimeTrackingHistoryQuery : IRequest<BaseResponse<List<TimeTrackingResponse>>>
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
} 