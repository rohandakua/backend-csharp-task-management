using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Break;
using PropVivo.Domain.Entities.TimeTracking;
using PropVivo.Domain.Enums;
using System.Net;

namespace PropVivo.Application.Features.Break.StartBreak
{
    public class StartBreakHandler : IRequestHandler<StartBreakCommand, BaseResponse<bool>>
    {
        private readonly IBreakRepository _breakRepository;
        private readonly ITimeTrackingRepository _timeTrackingRepository;

        public StartBreakHandler(IBreakRepository breakRepository, ITimeTrackingRepository timeTrackingRepository)
        {
            _breakRepository = breakRepository;
            _timeTrackingRepository = timeTrackingRepository;
        }

        public async Task<BaseResponse<bool>> Handle(StartBreakCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            // Check if user has active time tracking
            var activeTimeTracking = await _timeTrackingRepository.GetActiveByUserIdAsync(request.UserId);
            if (activeTimeTracking == null)
                throw new BadRequestException("No active time tracking session found. Please start time tracking first.");

            // Check if user already has an active break
            var hasActiveBreak = await _breakRepository.HasActiveBreakAsync(request.UserId);
            if (hasActiveBreak)
                throw new BadRequestException("You already have an active break. Please end the current break first.");

            var breakItem = new PropVivo.Domain.Entities.Break.Break
            {
                UserId = request.UserId,
                TimeTrackingId = activeTimeTracking.Id,
                StartTime = DateTime.UtcNow,
                Type = request.Type,
                Reason = request.Reason,
                Duration = 0,
                CreatedAt = DateTime.UtcNow
            };

            await _breakRepository.CreateAsync(breakItem);

            response.Data = true;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Break started successfully";

            return response;
        }
    }
} 