namespace PropVivo.Application.Dto.Report
{
    public class AttendanceReportResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal TotalHours { get; set; }
        public decimal WorkHours { get; set; }
        public decimal BreakHours { get; set; }
        public bool IsEightHourCompliant { get; set; }
    }
}