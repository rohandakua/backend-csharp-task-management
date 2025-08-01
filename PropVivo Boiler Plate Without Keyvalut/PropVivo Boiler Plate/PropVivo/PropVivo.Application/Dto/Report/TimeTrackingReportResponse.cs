namespace PropVivo.Application.Dto.Report
{
    public class TimeTrackingReportResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal TotalHours { get; set; }
        public decimal WorkHours { get; set; }
        public decimal BreakHours { get; set; }
        public bool IsEightHourCompliant { get; set; }
        public List<TimeTrackingEntry> Entries { get; set; } = new List<TimeTrackingEntry>();
    }

    public class TimeTrackingEntry
    {
        public string TaskId { get; set; } = string.Empty;
        public string TaskTitle { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal Duration { get; set; }
    }
}