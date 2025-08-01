using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Task;
using PropVivo.Domain.Enums;
using TaskStatus = PropVivo.Domain.Enums.TaskStatus;

namespace PropVivo.Application.Features.Task.UpdateTask
{
    public class UpdateTaskCommand : IRequest<BaseResponse<TaskResponse>>
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedHours { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskStatus Status { get; set; } = TaskStatus.Assigned;
    }
} 