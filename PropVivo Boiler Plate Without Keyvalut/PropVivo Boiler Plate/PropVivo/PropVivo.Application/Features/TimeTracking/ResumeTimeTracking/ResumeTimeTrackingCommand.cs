using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Features.TimeTracking.ResumeTimeTracking
{
    public class ResumeTimeTrackingCommand : IRequest<BaseResponse<bool>>
    {
        public string UserId { get; set; } = string.Empty;
    }
} 