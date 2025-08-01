using PropVivo.Domain.Common;
using PropVivo.Domain.Enums;

namespace PropVivo.Domain.Entities.Break
{
    public class Break : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string TimeTrackingId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public BreakType Type { get; set; } = BreakType.Regular;
        public string? Reason { get; set; }
        public decimal Duration { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 