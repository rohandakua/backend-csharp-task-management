using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Enums;
using System.Net;

namespace PropVivo.Application.Features.Task.CompleteTask
{
    public class CompleteTaskHandler : IRequestHandler<CompleteTaskCommand, BaseResponse<bool>>
    {
        private readonly ITaskRepository _taskRepository;

        public CompleteTaskHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<BaseResponse<bool>> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new NotFoundException($"Task with ID {request.TaskId} not found");

            if (task.AssignedToId != request.UserId)
                throw new BadRequestException("You can only complete tasks assigned to you");

            if (task.Status == Domain.Enums.TaskStatus.Completed)
                throw new BadRequestException("Task is already completed");

            task.Status = Domain.Enums.TaskStatus.Completed;
            task.CompletedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task);

            response.Data = true;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Task completed successfully";

            return response;
        }
    }
} 