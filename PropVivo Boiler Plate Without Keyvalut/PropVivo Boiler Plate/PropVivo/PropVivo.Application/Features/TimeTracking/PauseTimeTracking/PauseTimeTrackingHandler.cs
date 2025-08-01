using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.TimeTracking;
using PropVivo.Domain.Enums;
using System.Net;

namespace PropVivo.Application.Features.TimeTracking.PauseTimeTracking
{
    public class PauseTimeTrackingHandler : IRequestHandler<PauseTimeTrackingCommand, BaseResponse<bool>>
    {
        private readonly ITimeTrackingRepository _timeTrackingRepository;

        public PauseTimeTrackingHandler(ITimeTrackingRepository timeTrackingRepository)
        {
            _timeTrackingRepository = timeTrackingRepository;
        }

        public async Task<BaseResponse<bool>> Handle(PauseTimeTrackingCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            var activeTimeTracking = await _timeTrackingRepository.GetActiveByUserIdAsync(request.UserId);
            if (activeTimeTracking == null)
                throw new NotFoundException("No active time tracking session found");

            if (activeTimeTracking.Status != TimeTrackingStatus.Active)
                throw new BadRequestException("Time tracking is not active");

            activeTimeTracking.Status = TimeTrackingStatus.Paused;
            activeTimeTracking.UpdatedAt = DateTime.UtcNow;

            await _timeTrackingRepository.UpdateAsync(activeTimeTracking);

            response.Data = true;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Time tracking paused successfully";

            return response;
        }
    }
} 