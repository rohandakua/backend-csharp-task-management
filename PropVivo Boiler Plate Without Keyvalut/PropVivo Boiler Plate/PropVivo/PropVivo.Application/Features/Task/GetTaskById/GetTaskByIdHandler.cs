using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.Task;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using System.Net;

namespace PropVivo.Application.Features.Task.GetTaskById
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, BaseResponse<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetTaskByIdHandler(ITaskRepository taskRepository, IUserRepository userRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TaskResponse>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TaskResponse>();

            var task = await _taskRepository.GetByIdAsync(request.Id);
            if (task == null)
                throw new NotFoundException($"Task with ID {request.Id} not found");

            var assignedUser = await _userRepository.GetByIdAsync(task.AssignedToId);
            var assignedByUser = await _userRepository.GetByIdAsync(task.AssignedById);

            var taskResponse = new TaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                AssignedToId = task.AssignedToId,
                AssignedToName = assignedUser != null ? $"{assignedUser.FirstName} {assignedUser.LastName}" : "Unknown",
                AssignedById = task.AssignedById,
                AssignedByName = assignedByUser != null ? $"{assignedByUser.FirstName} {assignedByUser.LastName}" : "Unknown",
                EstimatedHours = task.EstimatedHours,
                Status = task.Status,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                StartedAt = task.StartedAt,
                CompletedAt = task.CompletedAt,
                IsSelfAdded = task.IsSelfAdded,
                TotalTimeSpent = 0, // TODO: Calculate from time tracking
                IsActive = task.Status == Domain.Enums.TaskStatus.InProgress
            };

            response.Data = taskResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Task retrieved successfully";

            return response;
        }
    }
} 