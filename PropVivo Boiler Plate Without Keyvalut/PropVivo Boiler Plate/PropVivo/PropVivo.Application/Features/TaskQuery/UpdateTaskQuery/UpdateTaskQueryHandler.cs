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

namespace PropVivo.Application.Features.TaskQuery.UpdateTaskQuery
{
    public class UpdateTaskQueryHandler : IRequestHandler<UpdateTaskQueryCommand, BaseResponse<TaskQueryResponse>>
    {
        private readonly ITaskQueryRepository _taskQueryRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateTaskQueryHandler(
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

        public async Task<BaseResponse<TaskQueryResponse>> Handle(UpdateTaskQueryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TaskQueryResponse>();

            var existingTaskQuery = await _taskQueryRepository.GetByIdAsync(request.Id);
            if (existingTaskQuery == null)
                throw new NotFoundException($"Task query with ID {request.Id} not found");

            // Validate assigned user if provided
            if (!string.IsNullOrEmpty(request.AssignedToId))
            {
                var assignedUser = await _userRepository.GetByIdAsync(request.AssignedToId);
                if (assignedUser == null)
                    throw new BadRequestException($"User with ID {request.AssignedToId} not found");
            }

            existingTaskQuery.Subject = request.Subject;
            existingTaskQuery.Description = request.Description;
            existingTaskQuery.Status = request.Status;
            existingTaskQuery.Priority = request.Priority;
            existingTaskQuery.AssignedToId = request.AssignedToId;
            existingTaskQuery.Resolution = request.Resolution;
            existingTaskQuery.Attachments = request.Attachments;

            // Update resolved info if status is resolved
            if (request.Status == QueryStatus.Resolved && !string.IsNullOrEmpty(request.Resolution))
            {
                existingTaskQuery.ResolvedAt = DateTime.UtcNow;
                // TODO: Set ResolvedById from current user context
            }

            var updatedTaskQuery = await _taskQueryRepository.UpdateAsync(existingTaskQuery);

            var task = await _taskRepository.GetByIdAsync(updatedTaskQuery.TaskId);
            var raisedByUser = await _userRepository.GetByIdAsync(updatedTaskQuery.RaisedById);
            var assignedToUser = !string.IsNullOrEmpty(updatedTaskQuery.AssignedToId) ? await _userRepository.GetByIdAsync(updatedTaskQuery.AssignedToId) : null;
            var resolvedByUser = !string.IsNullOrEmpty(updatedTaskQuery.ResolvedById) ? await _userRepository.GetByIdAsync(updatedTaskQuery.ResolvedById) : null;

            var taskQueryResponse = new TaskQueryResponse
            {
                Id = updatedTaskQuery.Id,
                TaskId = updatedTaskQuery.TaskId,
                TaskTitle = task?.Title ?? "Unknown Task",
                RaisedById = updatedTaskQuery.RaisedById,
                RaisedByName = raisedByUser != null ? $"{raisedByUser.FirstName} {raisedByUser.LastName}" : "Unknown",
                AssignedToId = updatedTaskQuery.AssignedToId,
                AssignedToName = assignedToUser != null ? $"{assignedToUser.FirstName} {assignedToUser.LastName}" : null,
                Subject = updatedTaskQuery.Subject,
                Description = updatedTaskQuery.Description,
                Status = updatedTaskQuery.Status,
                Priority = updatedTaskQuery.Priority,
                CreatedAt = updatedTaskQuery.CreatedAt,
                ResolvedAt = updatedTaskQuery.ResolvedAt,
                Resolution = updatedTaskQuery.Resolution,
                ResolvedById = updatedTaskQuery.ResolvedById,
                ResolvedByName = resolvedByUser != null ? $"{resolvedByUser.FirstName} {resolvedByUser.LastName}" : null,
                Attachments = updatedTaskQuery.Attachments
            };

            response.Data = taskQueryResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Task query updated successfully";

            return response;
        }
    }
} 