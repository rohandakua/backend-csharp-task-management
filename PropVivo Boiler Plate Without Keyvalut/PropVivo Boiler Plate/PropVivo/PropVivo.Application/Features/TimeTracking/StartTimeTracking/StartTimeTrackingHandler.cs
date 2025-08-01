using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.TimeTracking;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Entities.TimeTracking;
using PropVivo.Domain.Enums;
using System.Net;

namespace PropVivo.Application.Features.TimeTracking.StartTimeTracking
{
    public class StartTimeTrackingHandler : IRequestHandler<StartTimeTrackingCommand, BaseResponse<TimeTrackingResponse>>
    {
        private readonly ITimeTrackingRepository _timeTrackingRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public StartTimeTrackingHandler(ITimeTrackingRepository timeTrackingRepository, ITaskRepository taskRepository, IMapper mapper)
        {
            _timeTrackingRepository = timeTrackingRepository;
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TimeTrackingResponse>> Handle(StartTimeTrackingCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TimeTrackingResponse>();

            // Validate task exists and is assigned to user
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new NotFoundException($"Task with ID {request.TaskId} not found");

            if (task.AssignedToId != request.UserId)
                throw new BadRequestException("You can only track time for tasks assigned to you");

            // Check if user already has active time tracking
            var hasActiveTracking = await _timeTrackingRepository.HasActiveTrackingAsync(request.UserId);
            if (hasActiveTracking)
                throw new BadRequestException("You already have active time tracking. Please stop the current session first");

            var timeTracking = new PropVivo.Domain.Entities.TimeTracking.TimeTracking
            {
                UserId = request.UserId,
                TaskId = request.TaskId,
                Date = DateTime.Today,
                StartTime = DateTime.UtcNow,
                Status = TimeTrackingStatus.Active,
                TotalHours = 0,
                BreakHours = 0,
                WorkHours = 0,
                IsEightHourCompliant = false,
                CreatedAt = DateTime.UtcNow
            };

            var createdTimeTracking = await _timeTrackingRepository.CreateAsync(timeTracking);

            var timeTrackingResponse = new TimeTrackingResponse
            {
                Id = createdTimeTracking.Id,
                UserId = createdTimeTracking.UserId,
                TaskId = createdTimeTracking.TaskId,
                TaskTitle = task.Title,
                Date = createdTimeTracking.Date,
                StartTime = createdTimeTracking.StartTime,
                EndTime = createdTimeTracking.EndTime,
                Status = createdTimeTracking.Status,
                TotalHours = createdTimeTracking.TotalHours,
                BreakHours = createdTimeTracking.BreakHours,
                WorkHours = createdTimeTracking.WorkHours,
                IsEightHourCompliant = createdTimeTracking.IsEightHourCompliant,
                CreatedAt = createdTimeTracking.CreatedAt,
                UpdatedAt = createdTimeTracking.UpdatedAt,
                IsActive = true
            };

            response.Data = timeTrackingResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.Created;
            response.Message = "Time tracking started successfully";

            return response;
        }
    }
} 