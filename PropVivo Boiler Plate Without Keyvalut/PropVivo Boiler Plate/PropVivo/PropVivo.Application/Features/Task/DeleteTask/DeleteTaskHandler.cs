using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Repositories;
using System.Net;

namespace PropVivo.Application.Features.Task.DeleteTask
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, BaseResponse<bool>>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<BaseResponse<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            var existingTask = await _taskRepository.GetByIdAsync(request.Id);
            if (existingTask == null)
                throw new NotFoundException($"Task with ID {request.Id} not found");

            var result = await _taskRepository.DeleteAsync(request.Id);

            response.Data = result;
            response.Success = result;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = result ? "Task deleted successfully" : "Failed to delete task";

            return response;
        }
    }
} 