using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TaskQuery;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using System.Net;
using TaskQueryEntity = PropVivo.Domain.Entities.TaskQuery.TaskQuery;

namespace PropVivo.Application.Features.TaskQuery.GetAllTaskQueries
{
    public class GetAllTaskQueriesHandler : IRequestHandler<GetAllTaskQueriesQuery, BaseResponse<List<TaskQueryResponse>>>
    {
        private readonly ITaskQueryRepository _taskQueryRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAllTaskQueriesHandler(
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

        public async Task<BaseResponse<List<TaskQueryResponse>>> Handle(GetAllTaskQueriesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<TaskQueryResponse>>();

            List<TaskQueryEntity> taskQueries;

            if (!string.IsNullOrEmpty(request.TaskId))
            {
                taskQueries = await _taskQueryRepository.GetByTaskIdAsync(request.TaskId);
            }
            else if (!string.IsNullOrEmpty(request.RaisedById))
            {
                taskQueries = await _taskQueryRepository.GetByRaisedByIdAsync(request.RaisedById);
            }
            else if (!string.IsNullOrEmpty(request.AssignedToId))
            {
                taskQueries = await _taskQueryRepository.GetByAssignedToIdAsync(request.AssignedToId);
            }
            else
            {
                taskQueries = await _taskQueryRepository.GetAllAsync();
            }

            var taskQueryResponses = new List<TaskQueryResponse>();

            foreach (var taskQuery in taskQueries)
            {
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

                taskQueryResponses.Add(taskQueryResponse);
            }

            response.Data = taskQueryResponses;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Task queries retrieved successfully";

            return response;
        }
    }
} 