using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TimeTracking;

namespace PropVivo.Application.Features.TimeTracking.GetCurrentTimeTracking
{
    public class GetCurrentTimeTrackingQuery : IRequest<BaseResponse<TimeTrackingResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
} 