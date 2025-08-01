using System.ComponentModel.DataAnnotations;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.Task
{
    public class CreateTaskRequest
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string AssignedToId { get; set; } = string.Empty;
        
        [Required]
        [Range(0.1, 24.0)]
        public decimal EstimatedHours { get; set; }
        
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        
        public bool IsSelfAdded { get; set; } = false;
    }
} 