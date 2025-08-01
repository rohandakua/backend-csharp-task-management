namespace PropVivo.Application.Dto.Report
{
    public class TeamReportResponse
    {
        public string SuperiorId { get; set; } = string.Empty;
        public string SuperiorName { get; set; } = string.Empty;
        public int TotalTeamMembers { get; set; }
        public int ActiveMembers { get; set; }
        public decimal TeamTotalHours { get; set; }
        public decimal TeamAverageHours { get; set; }
        public int TeamComplianceRate { get; set; }
        public List<EmployeeReportResponse> TeamMembers { get; set; } = new List<EmployeeReportResponse>();
    }
}