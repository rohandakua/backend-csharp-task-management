using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.TaskQuery;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Entities.TaskQuery;
using System.Net;

namespace PropVivo.Application.Features.TaskQuery.GetTaskQueryById
{
    public class GetTaskQueryByIdHandler : IRequestHandler<GetTaskQueryByIdQuery, BaseResponse<TaskQueryResponse>>
    {
        private readonly ITaskQueryRepository _taskQueryRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetTaskQueryByIdHandler(
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

        public async Task<BaseResponse<TaskQueryResponse>> Handle(GetTaskQueryByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TaskQueryResponse>();

            var taskQuery = await _taskQueryRepository.GetByIdAsync(request.Id);
            if (taskQuery == null)
                throw new NotFoundException($"Task query with ID {request.Id} not found");

            var task = await _taskRepository.GetByIdAsync(taskQuery.TaskId);
            var raisedByUser = await _userRepository.GetByIdAsync(taskQuery.RaisedById);
            var assignedToUser = !string.IsNullOrEmpty(taskQuery.AssignedToId) ? await _userRepository.GetByIdAsync(taskQuery.AssignedToId) : null;
            var resolvedByUser = !string.IsNullOrEmpty(taskQuery.ResolvedById) ? await _userRepository.GetByIdAsync(taskQuery.ResolvedById) : null;

            var taskQueryResponse = new TaskQueryResponse
            {
                Id = taskQuery.Id,
                TaskId = taskQuery.TaskId,
                TaskTitle = task?.Title ?? "Unknown Task",
                RaisedById = taskQuery.RaisedById,
                RaisedByName = raisedByUser != null ? $"{raisedByUser.FirstName} {raisedByUser.LastName}" : "Unknown",
                AssignedToId = taskQuery.AssignedToId,
                AssignedToName = assignedToUser != null ? $"{assignedToUser.FirstName} {assignedToUser.LastName}" : null,
                Subject = taskQuery.Subject,
                Description = taskQuery.Description,
                Status = taskQuery.Status,
                Priority = taskQuery.Priority,
                CreatedAt = taskQuery.CreatedAt,
                ResolvedAt = taskQuery.ResolvedAt,
                Resolution = taskQuery.Resolution,
                ResolvedById = taskQuery.ResolvedById,
                ResolvedByName = resolvedByUser != null ? $"{resolvedByUser.FirstName} {resolvedByUser.LastName}" : null,
                Attachments = taskQuery.Attachments
            };

            response.Data = taskQueryResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Task query retrieved successfully";

            return response;
        }
    }
} 