using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.Break
{
    public class BreakResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TimeTrackingId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public BreakType Type { get; set; }
        public string? Reason { get; set; }
        public decimal Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
} 