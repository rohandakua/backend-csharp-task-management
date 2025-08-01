using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.Task;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using System.Net;

namespace PropVivo.Application.Features.Task.UpdateTask
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, BaseResponse<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateTaskHandler(ITaskRepository taskRepository, IUserRepository userRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TaskResponse>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TaskResponse>();

            var existingTask = await _taskRepository.GetByIdAsync(request.Id);
            if (existingTask == null)
                throw new NotFoundException($"Task with ID {request.Id} not found");

            existingTask.Title = request.Title;
            existingTask.Description = request.Description;
            existingTask.EstimatedHours = request.EstimatedHours;
            existingTask.Priority = request.Priority;
            existingTask.Status = request.Status;

            var updatedTask = await _taskRepository.UpdateAsync(existingTask);

            var assignedUser = await _userRepository.GetByIdAsync(updatedTask.AssignedToId);
            var assignedByUser = await _userRepository.GetByIdAsync(updatedTask.AssignedById);

            var taskResponse = new TaskResponse
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                Description = updatedTask.Description,
                AssignedToId = updatedTask.AssignedToId,
                AssignedToName = assignedUser != null ? $"{assignedUser.FirstName} {assignedUser.LastName}" : "Unknown",
                AssignedById = updatedTask.AssignedById,
                AssignedByName = assignedByUser != null ? $"{assignedByUser.FirstName} {assignedByUser.LastName}" : "Unknown",
                EstimatedHours = updatedTask.EstimatedHours,
                Status = updatedTask.Status,
                Priority = updatedTask.Priority,
                CreatedAt = updatedTask.CreatedAt,
                StartedAt = updatedTask.StartedAt,
                CompletedAt = updatedTask.CompletedAt,
                IsSelfAdded = updatedTask.IsSelfAdded,
                TotalTimeSpent = 0, // TODO: Calculate from time tracking
                IsActive = updatedTask.Status == Domain.Enums.TaskStatus.InProgress
            };

            response.Data = taskResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Task updated successfully";

            return response;
        }
    }
} 