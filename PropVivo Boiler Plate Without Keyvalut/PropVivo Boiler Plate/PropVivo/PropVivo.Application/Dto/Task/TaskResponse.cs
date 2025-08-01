using PropVivo.Domain.Enums;
using TaskStatus = PropVivo.Domain.Enums.TaskStatus;

namespace PropVivo.Application.Dto.Task
{
    public class TaskResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignedToId { get; set; } = string.Empty;
        public string AssignedToName { get; set; } = string.Empty;
        public string AssignedById { get; set; } = string.Empty;
        public string AssignedByName { get; set; } = string.Empty;
        public decimal EstimatedHours { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsSelfAdded { get; set; }
        public decimal TotalTimeSpent { get; set; }
        public bool IsActive { get; set; }
    }
} 