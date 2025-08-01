using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.TaskQuery;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Enums;
using System.Net;
using TaskQueryEntity = PropVivo.Domain.Entities.TaskQuery.TaskQuery;

namespace PropVivo.Application.Features.TaskQuery.ResolveTaskQuery
{
    public class ResolveTaskQueryHandler : IRequestHandler<ResolveTaskQueryCommand, BaseResponse<TaskQueryResponse>>
    {
        private readonly ITaskQueryRepository _taskQueryRepository;
        private readonly IMapper _mapper;

        public ResolveTaskQueryHandler(ITaskQueryRepository taskQueryRepository, IMapper mapper)
        {
            _taskQueryRepository = taskQueryRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TaskQueryResponse>> Handle(ResolveTaskQueryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TaskQueryResponse>();

            var taskQuery = await _taskQueryRepository.GetByIdAsync(request.Id);
            if (taskQuery == null)
                throw new NotFoundException($"Task query with ID {request.Id} not found");

            taskQuery.Status = QueryStatus.Resolved;
            taskQuery.Resolution = request.Resolution;
            taskQuery.ResolvedById = request.ResolvedById;
            taskQuery.ResolvedAt = DateTime.UtcNow;

            var updatedTaskQuery = await _taskQueryRepository.UpdateAsync(taskQuery);
            var taskQueryResponse = _mapper.Map<TaskQueryResponse>(updatedTaskQuery);

            response.Data = taskQueryResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Task query resolved successfully";

            return response;
        }
    }
}