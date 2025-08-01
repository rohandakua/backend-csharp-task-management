using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TimeTracking;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using System.Net;
using TimeTrackingEntity = PropVivo.Domain.Entities.TimeTracking.TimeTracking;

namespace PropVivo.Application.Features.TimeTracking.GetTimeTrackingHistory
{
    public class GetTimeTrackingHistoryHandler : IRequestHandler<GetTimeTrackingHistoryQuery, BaseResponse<List<TimeTrackingResponse>>>
    {
        private readonly ITimeTrackingRepository _timeTrackingRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetTimeTrackingHistoryHandler(ITimeTrackingRepository timeTrackingRepository, ITaskRepository taskRepository, IMapper mapper)
        {
            _timeTrackingRepository = timeTrackingRepository;
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<TimeTrackingResponse>>> Handle(GetTimeTrackingHistoryQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<TimeTrackingResponse>>();

            var timeTrackingHistory = await _timeTrackingRepository.GetByUserIdAndDateRangeAsync(request.UserId, request.StartDate, request.EndDate);

            var timeTrackingResponses = new List<TimeTrackingResponse>();

            foreach (TimeTrackingEntity timeTracking in timeTrackingHistory)
            {
                var task = await _taskRepository.GetByIdAsync(timeTracking.TaskId);

                var timeTrackingResponse = new TimeTrackingResponse
                {
                    Id = timeTracking.Id,
                    UserId = timeTracking.UserId,
                    TaskId = timeTracking.TaskId,
                    TaskTitle = task?.Title ?? "Unknown Task",
                    Date = timeTracking.Date,
                    StartTime = timeTracking.StartTime,
                    EndTime = timeTracking.EndTime,
                    Status = timeTracking.Status,
                    TotalHours = timeTracking.TotalHours,
                    BreakHours = timeTracking.BreakHours,
                    WorkHours = timeTracking.WorkHours,
                    IsEightHourCompliant = timeTracking.IsEightHourCompliant,
                    CreatedAt = timeTracking.CreatedAt,
                    UpdatedAt = timeTracking.UpdatedAt,
                    IsActive = timeTracking.Status == Domain.Enums.TimeTrackingStatus.Active
                };

                timeTrackingResponses.Add(timeTrackingResponse);
            }

            response.Data = timeTrackingResponses;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Time tracking history retrieved successfully";

            return response;
        }
    }
} 