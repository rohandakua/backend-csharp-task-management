using System.ComponentModel.DataAnnotations;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.TaskQuery
{
    public class UpdateTaskQueryRequest
    {
        [Required]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public QueryStatus Status { get; set; } = QueryStatus.Open;
        
        public QueryPriority Priority { get; set; } = QueryPriority.Medium;
        
        public string? AssignedToId { get; set; }
        
        public string? Resolution { get; set; }
        
        public List<string> Attachments { get; set; } = new List<string>();
    }
} 