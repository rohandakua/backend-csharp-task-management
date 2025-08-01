using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.TaskQuery
{
    public class TaskQueryResponse
    {
        public string Id { get; set; } = string.Empty;
        public string TaskId { get; set; } = string.Empty;
        public string TaskTitle { get; set; } = string.Empty;
        public string RaisedById { get; set; } = string.Empty;
        public string RaisedByName { get; set; } = string.Empty;
        public string? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public QueryStatus Status { get; set; }
        public QueryPriority Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? Resolution { get; set; }
        public string? ResolvedById { get; set; }
        public string? ResolvedByName { get; set; }
        public List<string> Attachments { get; set; } = new List<string>();
    }
} 