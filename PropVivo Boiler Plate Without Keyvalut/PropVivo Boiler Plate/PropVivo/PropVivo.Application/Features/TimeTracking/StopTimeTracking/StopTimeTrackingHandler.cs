using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.TimeTracking;
using PropVivo.Domain.Enums;
using System.Net;

namespace PropVivo.Application.Features.TimeTracking.StopTimeTracking
{
    public class StopTimeTrackingHandler : IRequestHandler<StopTimeTrackingCommand, BaseResponse<bool>>
    {
        private readonly ITimeTrackingRepository _timeTrackingRepository;

        public StopTimeTrackingHandler(ITimeTrackingRepository timeTrackingRepository)
        {
            _timeTrackingRepository = timeTrackingRepository;
        }

        public async Task<BaseResponse<bool>> Handle(StopTimeTrackingCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            var activeTimeTracking = await _timeTrackingRepository.GetActiveByUserIdAsync(request.UserId);
            if (activeTimeTracking == null)
                throw new NotFoundException("No active time tracking session found");

            activeTimeTracking.EndTime = DateTime.UtcNow;
            activeTimeTracking.Status = TimeTrackingStatus.Completed;
            activeTimeTracking.UpdatedAt = DateTime.UtcNow;

            // Calculate total hours
            var totalTime = activeTimeTracking.EndTime.Value - activeTimeTracking.StartTime;
            activeTimeTracking.TotalHours = (decimal)totalTime.TotalHours;

            // Calculate work hours (total - break hours)
            activeTimeTracking.WorkHours = activeTimeTracking.TotalHours - activeTimeTracking.BreakHours;

            // Check 8-hour compliance
            activeTimeTracking.IsEightHourCompliant = activeTimeTracking.WorkHours >= 8.0m;

            await _timeTrackingRepository.UpdateAsync(activeTimeTracking);

            response.Data = true;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Time tracking stopped successfully";

            return response;
        }
    }
} 