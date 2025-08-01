using PropVivo.Domain.Common;
using PropVivo.Domain.Enums;

namespace PropVivo.Domain.Entities.TimeTracking
{
    public class TimeTracking : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string TaskId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeTrackingStatus Status { get; set; } = TimeTrackingStatus.Active;
        public decimal TotalHours { get; set; }
        public decimal BreakHours { get; set; }
        public decimal WorkHours { get; set; }
        public bool IsEightHourCompliant { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
} 