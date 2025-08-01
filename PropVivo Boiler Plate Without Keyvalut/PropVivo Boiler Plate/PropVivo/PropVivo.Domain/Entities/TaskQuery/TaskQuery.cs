using PropVivo.Domain.Common;
using PropVivo.Domain.Enums;

namespace PropVivo.Domain.Entities.TaskQuery
{
    public class TaskQuery : BaseEntity
    {
        public string TaskId { get; set; } = string.Empty;
        public string RaisedById { get; set; } = string.Empty;
        public string? AssignedToId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public QueryStatus Status { get; set; } = QueryStatus.Open;
        public QueryPriority Priority { get; set; } = QueryPriority.Medium;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
        public string? Resolution { get; set; }
        public string? ResolvedById { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
    }
} 