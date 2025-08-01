using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Features.TimeTracking.StopTimeTracking
{
    public class StopTimeTrackingCommand : IRequest<BaseResponse<bool>>
    {
        public string UserId { get; set; } = string.Empty;
    }
} 