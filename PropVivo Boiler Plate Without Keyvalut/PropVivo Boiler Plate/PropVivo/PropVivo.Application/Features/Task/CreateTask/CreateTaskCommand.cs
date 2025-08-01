using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Task;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Features.Task.CreateTask
{
    public class CreateTaskCommand : IRequest<BaseResponse<TaskResponse>>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignedToId { get; set; } = string.Empty;
        public string AssignedById { get; set; } = string.Empty;
        public decimal EstimatedHours { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public bool IsSelfAdded { get; set; } = false;
    }
} 