using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Enums;
using System.Net;

namespace PropVivo.Application.Features.Task.StartTask
{
    public class StartTaskHandler : IRequestHandler<StartTaskCommand, BaseResponse<bool>>
    {
        private readonly ITaskRepository _taskRepository;

        public StartTaskHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<BaseResponse<bool>> Handle(StartTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new NotFoundException($"Task with ID {request.TaskId} not found");

            if (task.AssignedToId != request.UserId)
                throw new BadRequestException("You can only start tasks assigned to you");

            if (task.Status == Domain.Enums.TaskStatus.Completed)
                throw new BadRequestException("Cannot start a completed task");

            // Check if user has any other active tasks
            var hasActiveTask = await _taskRepository.HasActiveTaskAsync(request.UserId);
            if (hasActiveTask)
                throw new BadRequestException("You already have an active task. Please complete or pause the current task first");

            task.Status = Domain.Enums.TaskStatus.InProgress;
            task.StartedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task);

            response.Data = true;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Task started successfully";

            return response;
        }
    }
} 