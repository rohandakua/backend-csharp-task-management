using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Dashboard;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.User;
using System.Net;

namespace PropVivo.Application.Features.Dashboard.GetDashboard
{
    public class GetDashboardHandler : IRequestHandler<GetDashboardQuery, BaseResponse<DashboardResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeTrackingRepository _timeTrackingRepository;
        private readonly ITaskQueryRepository _taskQueryRepository;
        private readonly IMapper _mapper;

        public GetDashboardHandler(
            IUserRepository userRepository,
            ITaskRepository taskRepository,
            ITimeTrackingRepository timeTrackingRepository,
            ITaskQueryRepository taskQueryRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
            _timeTrackingRepository = timeTrackingRepository;
            _taskQueryRepository = taskQueryRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<DashboardResponse>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<DashboardResponse>();

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundException("User not found");

            // Get today's time tracking
            var todayTimeTracking = await _timeTrackingRepository.GetByUserIdAndDateAsync(request.UserId, DateTime.Today);
            var todayTracking = todayTimeTracking.FirstOrDefault();

            // Get assigned tasks
            var assignedTasks = await _taskRepository.GetByAssignedToIdAsync(request.UserId);

            // Get task queries
            var taskQueries = await _taskQueryRepository.GetByRaisedByIdAsync(request.UserId);

            // Calculate dashboard stats
            var stats = new DashboardStats
            {
                TotalTasks = assignedTasks.Count,
                CompletedTasks = assignedTasks.Count(t => t.Status == Domain.Enums.TaskStatus.Completed),
                PendingTasks = assignedTasks.Count(t => t.Status == Domain.Enums.TaskStatus.Assigned),
                ActiveQueries = taskQueries.Count(q => q.Status == Domain.Enums.QueryStatus.Open || q.Status == Domain.Enums.QueryStatus.InProgress),
                ResolvedQueries = taskQueries.Count(q => q.Status == Domain.Enums.QueryStatus.Resolved),
                AverageHoursPerDay = todayTracking?.WorkHours ?? 0,
                DaysWorkedThisWeek = 0 // TODO: Calculate from time tracking history
            };

            var dashboardResponse = new DashboardResponse
            {
                UserId = user.Id,
                UserName = $"{user.FirstName} {user.LastName}",
                Role = user.Role,
                CurrentDate = DateTime.Today,
                TotalHoursWorkedToday = todayTracking?.WorkHours ?? 0,
                BreakHoursToday = todayTracking?.BreakHours ?? 0,
                IsEightHourCompliant = todayTracking?.IsEightHourCompliant ?? false,
                ActiveTimeTracking = todayTracking != null ? new Application.Dto.TimeTracking.TimeTrackingResponse
                {
                    Id = todayTracking.Id,
                    UserId = todayTracking.UserId,
                    TaskId = todayTracking.TaskId,
                    TaskTitle = "Current Task", // TODO: Get task title
                    Date = todayTracking.Date,
                    StartTime = todayTracking.StartTime,
                    EndTime = todayTracking.EndTime,
                    Status = todayTracking.Status,
                    TotalHours = todayTracking.TotalHours,
                    BreakHours = todayTracking.BreakHours,
                    WorkHours = todayTracking.WorkHours,
                    IsEightHourCompliant = todayTracking.IsEightHourCompliant,
                    CreatedAt = todayTracking.CreatedAt,
                    UpdatedAt = todayTracking.UpdatedAt,
                    IsActive = todayTracking.Status == Domain.Enums.TimeTrackingStatus.Active
                } : null,
                AssignedTasks = new List<Application.Dto.Task.TaskResponse>(), // TODO: Map assigned tasks
                TaskQueries = new List<Application.Dto.TaskQuery.TaskQueryResponse>(), // TODO: Map task queries
                Stats = stats
            };

            response.Data = dashboardResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Dashboard data retrieved successfully";

            return response;
        }
    }
} 