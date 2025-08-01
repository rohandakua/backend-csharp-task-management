using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Break;
using PropVivo.Domain.Entities.TimeTracking;
using System.Net;

namespace PropVivo.Application.Features.Break.EndBreak
{
    public class EndBreakHandler : IRequestHandler<EndBreakCommand, BaseResponse<bool>>
    {
        private readonly IBreakRepository _breakRepository;
        private readonly ITimeTrackingRepository _timeTrackingRepository;

        public EndBreakHandler(IBreakRepository breakRepository, ITimeTrackingRepository timeTrackingRepository)
        {
            _breakRepository = breakRepository;
            _timeTrackingRepository = timeTrackingRepository;
        }

        public async Task<BaseResponse<bool>> Handle(EndBreakCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            var activeBreak = await _breakRepository.GetActiveByUserIdAsync(request.UserId);
            if (activeBreak == null)
                throw new NotFoundException("No active break found");

            activeBreak.EndTime = DateTime.UtcNow;
            var breakDuration = activeBreak.EndTime.Value - activeBreak.StartTime;
            activeBreak.Duration = (decimal)breakDuration.TotalHours;

            await _breakRepository.UpdateAsync(activeBreak);

            // Update time tracking with break hours
            var activeTimeTracking = await _timeTrackingRepository.GetActiveByUserIdAsync(request.UserId);
            if (activeTimeTracking != null)
            {
                activeTimeTracking.BreakHours += activeBreak.Duration;
                activeTimeTracking.WorkHours = activeTimeTracking.TotalHours - activeTimeTracking.BreakHours;
                activeTimeTracking.IsEightHourCompliant = activeTimeTracking.WorkHours >= 8.0m;
                activeTimeTracking.UpdatedAt = DateTime.UtcNow;

                await _timeTrackingRepository.UpdateAsync(activeTimeTracking);
            }

            response.Data = true;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Break ended successfully";

            return response;
        }
    }
} 