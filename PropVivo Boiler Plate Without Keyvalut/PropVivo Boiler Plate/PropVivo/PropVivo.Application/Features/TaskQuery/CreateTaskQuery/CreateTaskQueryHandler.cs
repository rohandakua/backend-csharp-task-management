using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.TaskQuery;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Entities.TaskQuery;
using PropVivo.Domain.Enums;
using System.Net;

namespace PropVivo.Application.Features.TaskQuery.CreateTaskQuery
{
    public class CreateTaskQueryHandler : IRequestHandler<CreateTaskQueryCommand, BaseResponse<TaskQueryResponse>>
    {
        private readonly ITaskQueryRepository _taskQueryRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateTaskQueryHandler(
            ITaskQueryRepository taskQueryRepository,
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _taskQueryRepository = taskQueryRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TaskQueryResponse>> Handle(CreateTaskQueryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TaskQueryResponse>();

            // Validate task exists
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new NotFoundException($"Task with ID {request.TaskId} not found");

            // Validate user exists
            var user = await _userRepository.GetByIdAsync(request.RaisedById);
            if (user == null)
                throw new NotFoundException($"User with ID {request.RaisedById} not found");

            // Validate user is assigned to the task
            if (task.AssignedToId != request.RaisedById)
                throw new BadRequestException("You can only create queries for tasks assigned to you");

            var taskQuery = new PropVivo.Domain.Entities.TaskQuery.TaskQuery
            {
                TaskId = request.TaskId,
                RaisedById = request.RaisedById,
                Subject = request.Subject,
                Description = request.Description,
                Status = QueryStatus.Open,
                Priority = request.Priority,
                Attachments = request.Attachments,
                CreatedAt = DateTime.UtcNow
            };

            var createdTaskQuery = await _taskQueryRepository.CreateAsync(taskQuery);

            var taskQueryResponse = new TaskQueryResponse
            {
                Id = createdTaskQuery.Id,
                TaskId = createdTaskQuery.TaskId,
                TaskTitle = task.Title,
                RaisedById = createdTaskQuery.RaisedById,
                RaisedByName = $"{user.FirstName} {user.LastName}",
                AssignedToId = createdTaskQuery.AssignedToId,
                AssignedToName = null, // TODO: Get assigned user name if assigned
                Subject = createdTaskQuery.Subject,
                Description = createdTaskQuery.Description,
                Status = createdTaskQuery.Status,
                Priority = createdTaskQuery.Priority,
                CreatedAt = createdTaskQuery.CreatedAt,
                ResolvedAt = createdTaskQuery.ResolvedAt,
                Resolution = createdTaskQuery.Resolution,
                ResolvedById = createdTaskQuery.ResolvedById,
                ResolvedByName = null, // TODO: Get resolver name if resolved
                Attachments = createdTaskQuery.Attachments
            };

            response.Data = taskQueryResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.Created;
            response.Message = "Task query created successfully";

            return response;
        }
    }
} 