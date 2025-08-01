using System.ComponentModel.DataAnnotations;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.TaskQuery
{
    public class CreateTaskQueryRequest
    {
        [Required]
        public string TaskId { get; set; } = string.Empty;
        
        [Required]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public QueryPriority Priority { get; set; } = QueryPriority.Medium;
        
        public List<string> Attachments { get; set; } = new List<string>();
    }
} 