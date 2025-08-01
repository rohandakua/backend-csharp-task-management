using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Dto.TimeTracking;
using PropVivo.Application.Repositories;
using System.Net;
using TimeTrackingEntity = PropVivo.Domain.Entities.TimeTracking.TimeTracking;

namespace PropVivo.Application.Features.TimeTracking.GetTimeTracking
{
    public class GetTimeTrackingHandler : IRequestHandler<GetTimeTrackingQuery, BaseResponse<TimeTrackingResponse>>
    {
        private readonly ITimeTrackingRepository _timeTrackingRepository;
        private readonly IMapper _mapper;

        public GetTimeTrackingHandler(ITimeTrackingRepository timeTrackingRepository, IMapper mapper)
        {
            _timeTrackingRepository = timeTrackingRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TimeTrackingResponse>> Handle(GetTimeTrackingQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<TimeTrackingResponse>();

            var activeTimeTracking = await _timeTrackingRepository.GetActiveByUserIdAsync(request.UserId);
            if (activeTimeTracking == null)
                throw new NotFoundException("No active time tracking found for the user");

            var timeTrackingResponse = _mapper.Map<TimeTrackingResponse>(activeTimeTracking);

            response.Data = timeTrackingResponse;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Active time tracking retrieved successfully";

            return response;
        }
    }
}