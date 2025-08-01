namespace PropVivo.Application.Dto.Report
{
    public class EmployeeReportResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalTasksAssigned { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public decimal TotalHoursWorked { get; set; }
        public decimal AverageHoursPerDay { get; set; }
        public int ComplianceDays { get; set; }
        public int TotalWorkingDays { get; set; }
    }
}