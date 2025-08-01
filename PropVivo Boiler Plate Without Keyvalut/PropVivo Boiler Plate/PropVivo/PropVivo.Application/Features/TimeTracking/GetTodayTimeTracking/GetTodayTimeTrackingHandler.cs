using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TimeTracking;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Entities.TimeTracking;
using System.Net;

namespace PropVivo.Application.Features.TimeTracking.GetTodayTimeTracking
{
    public class GetTodayTimeTrackingHandler : IRequestHandler<GetTodayTimeTrackingQuery, BaseResponse<TimeTrackingResponse>>
    {
        private readonly ITimeTrackingRepository _timeTrackingRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetTodayTimeTrackingHandler(ITimeTrackingRepository timeTrackingRepository, ITaskRepository taskRepository, IMapper mapper)
        {
            _timeTrackingRepository = timeTrackingRepository;
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TimeTrackingResponse>> Handle(GetTodayTimeTrackingQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TimeTrackingResponse>();

            var todayTimeTracking = await _timeTrackingRepository.GetByUserIdAndDateAsync(request.UserId, DateTime.Today);
            var todayTracking = todayTimeTracking.FirstOrDefault();

            if (todayTracking == null)
            {
                response.Data = null;
                response.Success = true;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "No time tracking found for today";
                return response;
            }

            var task = await _taskRepository.GetByIdAsync(todayTracking.TaskId);

            var timeTrackingResponse = new TimeTrackingResponse
            {
                Id = todayTracking.Id,
                UserId = todayTracking.UserId,
                TaskId = todayTracking.TaskId,
                TaskTitle = task?.Title ?? "Unknown Task",
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
            };

            response.Data = timeTrackingResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Today's time tracking retrieved successfully";

            return response;
        }
    }
} 