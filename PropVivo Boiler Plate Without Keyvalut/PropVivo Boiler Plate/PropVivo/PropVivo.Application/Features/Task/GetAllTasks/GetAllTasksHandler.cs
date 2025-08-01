using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Task;
using PropVivo.Application.Repositories;
using TaskEntity = PropVivo.Domain.Entities.Task.Task;
using System.Net;

namespace PropVivo.Application.Features.Task.GetAllTasks
{
    public class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, BaseResponse<List<TaskResponse>>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllTasksHandler(ITaskRepository taskRepository, IUserRepository userRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<TaskResponse>>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<TaskResponse>>();

            List<TaskEntity> tasks;
            if (!string.IsNullOrEmpty(request.AssignedToId))
            {
                tasks = await _taskRepository.GetByAssignedToIdAsync(request.AssignedToId);
            }
            else
            {
                tasks = await _taskRepository.GetAllAsync();
            }

            var taskResponses = new List<TaskResponse>();

            foreach (var task in tasks)
            {
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

                taskResponses.Add(taskResponse);
            }

            response.Data = taskResponses;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Tasks retrieved successfully";

            return response;
        }
    }
} 