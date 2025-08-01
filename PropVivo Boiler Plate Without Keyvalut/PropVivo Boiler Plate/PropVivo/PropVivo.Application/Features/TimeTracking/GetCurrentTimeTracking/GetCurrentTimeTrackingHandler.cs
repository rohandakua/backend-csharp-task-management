using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TimeTracking;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Entities.TimeTracking;
using System.Net;

namespace PropVivo.Application.Features.TimeTracking.GetCurrentTimeTracking
{
    public class GetCurrentTimeTrackingHandler : IRequestHandler<GetCurrentTimeTrackingQuery, BaseResponse<TimeTrackingResponse>>
    {
        private readonly ITimeTrackingRepository _timeTrackingRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetCurrentTimeTrackingHandler(ITimeTrackingRepository timeTrackingRepository, ITaskRepository taskRepository, IMapper mapper)
        {
            _timeTrackingRepository = timeTrackingRepository;
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TimeTrackingResponse>> Handle(GetCurrentTimeTrackingQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TimeTrackingResponse>();

            var currentTimeTracking = await _timeTrackingRepository.GetActiveByUserIdAsync(request.UserId);
            if (currentTimeTracking == null)
            {
                response.Data = null;
                response.Success = true;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "No active time tracking session found";
                return response;
            }

            var task = await _taskRepository.GetByIdAsync(currentTimeTracking.TaskId);

            var timeTrackingResponse = new TimeTrackingResponse
            {
                Id = currentTimeTracking.Id,
                UserId = currentTimeTracking.UserId,
                TaskId = currentTimeTracking.TaskId,
                TaskTitle = task?.Title ?? "Unknown Task",
                Date = currentTimeTracking.Date,
                StartTime = currentTimeTracking.StartTime,
                EndTime = currentTimeTracking.EndTime,
                Status = currentTimeTracking.Status,
                TotalHours = currentTimeTracking.TotalHours,
                BreakHours = currentTimeTracking.BreakHours,
                WorkHours = currentTimeTracking.WorkHours,
                IsEightHourCompliant = currentTimeTracking.IsEightHourCompliant,
                CreatedAt = currentTimeTracking.CreatedAt,
                UpdatedAt = currentTimeTracking.UpdatedAt,
                IsActive = currentTimeTracking.Status == Domain.Enums.TimeTrackingStatus.Active
            };

            response.Data = timeTrackingResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Current time tracking retrieved successfully";

            return response;
        }
    }
} 