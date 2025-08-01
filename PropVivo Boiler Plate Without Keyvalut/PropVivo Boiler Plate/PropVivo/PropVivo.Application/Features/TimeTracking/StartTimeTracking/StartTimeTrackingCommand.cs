using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TimeTracking;

namespace PropVivo.Application.Features.TimeTracking.StartTimeTracking
{
    public class StartTimeTrackingCommand : IRequest<BaseResponse<TimeTrackingResponse>>
    {
        public string TaskId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
} 