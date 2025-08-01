using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Features.TimeTracking.PauseTimeTracking
{
    public class PauseTimeTrackingCommand : IRequest<BaseResponse<bool>>
    {
        public string UserId { get; set; } = string.Empty;
    }
} 