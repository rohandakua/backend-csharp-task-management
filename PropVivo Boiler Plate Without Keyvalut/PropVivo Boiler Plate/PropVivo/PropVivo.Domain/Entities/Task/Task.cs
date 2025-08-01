using PropVivo.Domain.Common;
using PropVivo.Domain.Enums;
using TaskStatus = PropVivo.Domain.Enums.TaskStatus;

namespace PropVivo.Domain.Entities.Task
{
    public class Task : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AssignedToId { get; set; } = string.Empty;
        public string AssignedById { get; set; } = string.Empty;
        public decimal EstimatedHours { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Assigned;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsSelfAdded { get; set; } = false;
    }
} 