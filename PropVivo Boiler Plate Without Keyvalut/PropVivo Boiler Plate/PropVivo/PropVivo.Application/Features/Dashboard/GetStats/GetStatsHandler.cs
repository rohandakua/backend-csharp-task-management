// using AutoMapper;
// using MediatR;
// using PropVivo.Application.Common.Base;
// using PropVivo.Application.Dto.Dashboard;
// using PropVivo.Application.Repositories;
// using System.Net;

// namespace PropVivo.Application.Features.Dashboard.GetStats
// {
//     public class GetStatsHandler : IRequestHandler<GetStatsQuery, BaseResponse<DashboardStats>>
//     {
//         private readonly ITaskRepository _taskRepository;
//         private readonly ITimeTrackingRepository _timeTrackingRepository;
//         private readonly ITaskQueryRepository _taskQueryRepository;
//         private readonly IMapper _mapper;

//         public GetStatsHandler(
//             ITaskRepository taskRepository,
//             ITimeTrackingRepository timeTrackingRepository,
//             ITaskQueryRepository taskQueryRepository,
//             IMapper mapper)
//         {
//             _taskRepository = taskRepository;
//             _timeTrackingRepository = timeTrackingRepository;
//             _taskQueryRepository = taskQueryRepository;
//             _mapper = mapper;
//         }

//         // public async Task<BaseResponse<DashboardStats>> Handle(GetStatsQuery request, CancellationToken cancellationToken)
//         // {
//         //     var response = new BaseResponse<DashboardStats>();

//         //     // Get tasks for the user
//         //     var tasks = await _taskRepository.GetByAssignedToIdAsync(request.UserId);

//         //     // Get time tracking for the date range
//         //     var timeTracking = await _timeTrackingRepository.GetByUserIdAndDateRangeAsync(request.UserId, request.StartDate, request.EndDate);

//         //     // Get task queries for the user
//         //     var taskQueries = await _taskQueryRepository.GetByRaisedByIdAsync(request.UserId);

//         //     // Calculate stats
//         //     var totalHours = timeTracking.Sum(t => t.WorkHours);
//         //     var daysWorked = timeTracking.Count();
//         //     var averageHoursPerDay = daysWorked > 0 ? totalHours / daysWorked : 0;

//         //     var stats = new DashboardStats
//         //     {
//         //         TotalTasks = tasks.Count,
//         //         CompletedTasks = tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Completed),
//         //         PendingTasks = tasks.Count(t => t.Status == Domain.Enums.TaskStatus.Assigned),
//         //         ActiveQueries = taskQueries.Count(q => q.Status == Domain.Enums.QueryStatus.Open || q.Status == Domain.Enums.QueryStatus.InProgress),
//         //         ResolvedQueries = taskQueries.Count(q => q.Status == Domain.Enums.QueryStatus.Resolved),
//         //         AverageHoursPerDay = averageHoursPerDay,
//         //         DaysWorkedThisWeek = daysWorked
//         //     };

//         //     response.Data = stats;
//         //     response.Success = true;
//         //     response.StatusCode = (int)HttpStatusCode.OK;
//         //     response.Message = "Stats retrieved successfully";

//         //     return response;
//         // }
//     }
// } 