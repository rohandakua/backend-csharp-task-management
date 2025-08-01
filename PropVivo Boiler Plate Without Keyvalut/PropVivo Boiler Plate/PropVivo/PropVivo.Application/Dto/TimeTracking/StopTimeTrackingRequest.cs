using System.ComponentModel.DataAnnotations;

namespace PropVivo.Application.Dto.TimeTracking
{
    public class StopTimeTrackingRequest
    {
        [Required]
        public string TaskId { get; set; } = string.Empty;
    }
} 