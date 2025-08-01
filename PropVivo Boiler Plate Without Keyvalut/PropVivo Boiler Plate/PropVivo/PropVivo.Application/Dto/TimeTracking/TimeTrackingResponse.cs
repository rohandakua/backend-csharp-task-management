using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.TimeTracking
{
    public class TimeTrackingResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TaskId { get; set; } = string.Empty;
        public string TaskTitle { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TimeTrackingStatus Status { get; set; }
        public decimal TotalHours { get; set; }
        public decimal BreakHours { get; set; }
        public decimal WorkHours { get; set; }
        public bool IsEightHourCompliant { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
} 