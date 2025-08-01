using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.Task;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Enums;
using System.Net;

namespace PropVivo.Application.Features.Task.CreateTask
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, BaseResponse<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateTaskHandler(ITaskRepository taskRepository, IUserRepository userRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TaskResponse>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TaskResponse>();

            // Validate assigned user exists
            var assignedUser = await _userRepository.GetByIdAsync(request.AssignedToId);
            if (assignedUser == null)
                throw new NotFoundException($"User with ID {request.AssignedToId} not found");

            // Validate assigned by user exists
            var assignedByUser = await _userRepository.GetByIdAsync(request.AssignedById);
            if (assignedByUser == null)
                throw new NotFoundException($"User with ID {request.AssignedById} not found");

            var task = new PropVivo.Domain.Entities.Task.Task
            {
                Title = request.Title,
                Description = request.Description,
                AssignedToId = request.AssignedToId,
                AssignedById = request.AssignedById,
                EstimatedHours = request.EstimatedHours,
                Priority = request.Priority,
                Status = Domain.Enums.TaskStatus.Assigned,
                IsSelfAdded = request.IsSelfAdded,
                CreatedAt = DateTime.UtcNow
            };

            var createdTask = await _taskRepository.CreateAsync(task);

            var taskResponse = new TaskResponse
            {
                Id = createdTask.Id,
                Title = createdTask.Title,
                Description = createdTask.Description,
                AssignedToId = createdTask.AssignedToId,
                AssignedToName = $"{assignedUser.FirstName} {assignedUser.LastName}",
                AssignedById = createdTask.AssignedById,
                AssignedByName = $"{assignedByUser.FirstName} {assignedByUser.LastName}",
                EstimatedHours = createdTask.EstimatedHours,
                Status = createdTask.Status,
                Priority = createdTask.Priority,
                CreatedAt = createdTask.CreatedAt,
                StartedAt = createdTask.StartedAt,
                CompletedAt = createdTask.CompletedAt,
                IsSelfAdded = createdTask.IsSelfAdded,
                TotalTimeSpent = 0,
                IsActive = false
            };

            response.Data = taskResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.Created;
            response.Message = "Task created successfully";

            return response;
        }
    }
} 