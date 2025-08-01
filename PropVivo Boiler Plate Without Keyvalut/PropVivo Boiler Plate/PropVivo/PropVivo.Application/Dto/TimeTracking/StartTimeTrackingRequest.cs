using System.ComponentModel.DataAnnotations;

namespace PropVivo.Application.Dto.TimeTracking
{
    public class StartTimeTrackingRequest
    {
        [Required]
        public string TaskId { get; set; } = string.Empty;
    }
} 