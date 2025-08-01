using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.Dashboard;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.User;
using System.Net;

namespace PropVivo.Application.Features.Dashboard.GetSuperiorDashboard
{
    public class GetSuperiorDashboardHandler : IRequestHandler<GetSuperiorDashboardQuery, BaseResponse<DashboardResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeTrackingRepository _timeTrackingRepository;
        private readonly ITaskQueryRepository _taskQueryRepository;
        private readonly IMapper _mapper;

        public GetSuperiorDashboardHandler(
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

        public async Task<BaseResponse<DashboardResponse>> Handle(GetSuperiorDashboardQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<DashboardResponse>();

            var superior = await _userRepository.GetByIdAsync(request.SuperiorId);
            if (superior == null)
                throw new NotFoundException("Superior not found");

            // Get team members
            var teamMembers = await _userRepository.GetEmployeesBySuperiorIdAsync(request.SuperiorId);

            // Get tasks assigned by superior
            var assignedTasks = await _taskRepository.GetByAssignedByIdAsync(request.SuperiorId);

            // Get task queries assigned to superior
            var taskQueries = await _taskQueryRepository.GetByAssignedToIdAsync(request.SuperiorId);

            // Calculate dashboard stats
            var stats = new DashboardStats
            {
                TotalTasks = assignedTasks.Count,
                CompletedTasks = assignedTasks.Count(t => t.Status == Domain.Enums.TaskStatus.Completed),
                PendingTasks = assignedTasks.Count(t => t.Status == Domain.Enums.TaskStatus.Assigned),
                ActiveQueries = taskQueries.Count(q => q.Status == Domain.Enums.QueryStatus.Open || q.Status == Domain.Enums.QueryStatus.InProgress),
                ResolvedQueries = taskQueries.Count(q => q.Status == Domain.Enums.QueryStatus.Resolved),
                AverageHoursPerDay = 0, // TODO: Calculate average from team members
                DaysWorkedThisWeek = 0 // TODO: Calculate from team time tracking
            };

            var dashboardResponse = new DashboardResponse
            {
                UserId = superior.Id,
                UserName = $"{superior.FirstName} {superior.LastName}",
                Role = superior.Role,
                CurrentDate = DateTime.Today,
                TotalHoursWorkedToday = 0, // TODO: Calculate from team members
                BreakHoursToday = 0, // TODO: Calculate from team members
                IsEightHourCompliant = true, // TODO: Calculate from team members
                ActiveTimeTracking = null, // Superior doesn't have active time tracking
                AssignedTasks = new List<Application.Dto.Task.TaskResponse>(), // TODO: Map assigned tasks
                TaskQueries = new List<Application.Dto.TaskQuery.TaskQueryResponse>(), // TODO: Map task queries
                Stats = stats
            };

            response.Data = dashboardResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Superior dashboard data retrieved successfully";

            return response;
        }
    }
} 