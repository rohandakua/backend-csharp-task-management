using PropVivo.Application.Dto.Task;
using PropVivo.Application.Dto.TaskQuery;
using PropVivo.Application.Dto.TimeTracking;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.Dashboard
{
    public class DashboardResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CurrentDate { get; set; }
        public decimal TotalHoursWorkedToday { get; set; }
        public decimal BreakHoursToday { get; set; }
        public bool IsEightHourCompliant { get; set; }
        public TimeTrackingResponse? ActiveTimeTracking { get; set; }
        public List<TaskResponse> AssignedTasks { get; set; } = new List<TaskResponse>();
        public List<TaskQueryResponse> TaskQueries { get; set; } = new List<TaskQueryResponse>();
        public DashboardStats Stats { get; set; } = new DashboardStats();
    }

    public class DashboardStats
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
        public int ActiveQueries { get; set; }
        public int ResolvedQueries { get; set; }
        public decimal AverageHoursPerDay { get; set; }
        public int DaysWorkedThisWeek { get; set; }
    }
} 