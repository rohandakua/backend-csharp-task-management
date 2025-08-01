namespace PropVivo.Application.Dto.Report
{
    public class TaskReportResponse
    {
        public string TaskId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string AssignedToName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal TotalTimeSpent { get; set; }
        public decimal EstimatedHours { get; set; }
    }
}