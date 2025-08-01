using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.TimeTracking;
using PropVivo.Domain.Enums;
using System.Net;

namespace PropVivo.Application.Features.TimeTracking.ResumeTimeTracking
{
    public class ResumeTimeTrackingHandler : IRequestHandler<ResumeTimeTrackingCommand, BaseResponse<bool>>
    {
        private readonly ITimeTrackingRepository _timeTrackingRepository;

        public ResumeTimeTrackingHandler(ITimeTrackingRepository timeTrackingRepository)
        {
            _timeTrackingRepository = timeTrackingRepository;
        }

        public async Task<BaseResponse<bool>> Handle(ResumeTimeTrackingCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            var pausedTimeTracking = await _timeTrackingRepository.GetActiveByUserIdAsync(request.UserId);
            if (pausedTimeTracking == null)
                throw new NotFoundException("No paused time tracking session found");

            if (pausedTimeTracking.Status != TimeTrackingStatus.Paused)
                throw new BadRequestException("Time tracking is not paused");

            pausedTimeTracking.Status = TimeTrackingStatus.Active;
            pausedTimeTracking.UpdatedAt = DateTime.UtcNow;

            await _timeTrackingRepository.UpdateAsync(pausedTimeTracking);

            response.Data = true;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Time tracking resumed successfully";

            return response;
        }
    }
} 