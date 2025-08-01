using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Features.Task.StartTask
{
    public class StartTaskCommand : IRequest<BaseResponse<bool>>
    {
        public string TaskId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
} 