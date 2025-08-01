using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.TaskQuery;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Entities.TaskQuery;
using System.Net;

namespace PropVivo.Application.Features.TaskQuery.GetSuperiorTaskQueries
{
    public class GetSuperiorTaskQueriesHandler : IRequestHandler<GetSuperiorTaskQueriesQuery, BaseResponse<List<TaskQueryResponse>>>
    {
        private readonly ITaskQueryRepository _taskQueryRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetSuperiorTaskQueriesHandler(
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

        public async Task<BaseResponse<List<TaskQueryResponse>>> Handle(GetSuperiorTaskQueriesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<TaskQueryResponse>>();

            // Get all task queries assigned to the superior
            var taskQueries = await _taskQueryRepository.GetByAssignedToIdAsync(request.SuperiorId);

            // Filter by status if provided
            if (request.Status.HasValue)
            {
                taskQueries = taskQueries.Where(q => q.Status == request.Status.Value).ToList();
            }

            // Filter by priority if provided
            if (request.Priority.HasValue)
            {
                taskQueries = taskQueries.Where(q => q.Priority == request.Priority.Value).ToList();
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
            response.Message = "Superior task queries retrieved successfully";

            return response;
        }
    }
} 