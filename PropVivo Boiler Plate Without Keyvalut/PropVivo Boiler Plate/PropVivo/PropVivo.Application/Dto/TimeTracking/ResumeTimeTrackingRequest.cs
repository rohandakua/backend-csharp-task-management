using System.ComponentModel.DataAnnotations;

namespace PropVivo.Application.Dto.TimeTracking
{
    public class ResumeTimeTrackingRequest
    {
        [Required]
        public string TaskId { get; set; } = string.Empty;
    }
} 