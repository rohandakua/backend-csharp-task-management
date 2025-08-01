using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Features.Task.CompleteTask
{
    public class CompleteTaskCommand : IRequest<BaseResponse<bool>>
    {
        public string TaskId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
} 