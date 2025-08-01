using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Break;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Break;
using System.Net;

namespace PropVivo.Application.Features.Break.GetTodayBreaks
{
    public class GetTodayBreaksHandler : IRequestHandler<GetTodayBreaksQuery, BaseResponse<List<BreakResponse>>>
    {
        private readonly IBreakRepository _breakRepository;
        private readonly IMapper _mapper;

        public GetTodayBreaksHandler(IBreakRepository breakRepository, IMapper mapper)
        {
            _breakRepository = breakRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<BreakResponse>>> Handle(GetTodayBreaksQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<BreakResponse>>();

            var todayBreaks = await _breakRepository.GetByUserIdAndDateAsync(request.UserId, DateTime.Today);

            var breakResponses = new List<BreakResponse>();

            foreach (var breakItem in todayBreaks)
            {
                var breakResponse = new BreakResponse
                {
                    Id = breakItem.Id,
                    UserId = breakItem.UserId,
                    TimeTrackingId = breakItem.TimeTrackingId,
                    StartTime = breakItem.StartTime,
                    EndTime = breakItem.EndTime,
                    Type = breakItem.Type,
                    Reason = breakItem.Reason,
                    Duration = breakItem.Duration,
                    CreatedAt = breakItem.CreatedAt,
                    IsActive = breakItem.EndTime == null
                };

                breakResponses.Add(breakResponse);
            }

            response.Data = breakResponses;
            response.Success = true;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "Today's breaks retrieved successfully";

            return response;
        }
    }
} 