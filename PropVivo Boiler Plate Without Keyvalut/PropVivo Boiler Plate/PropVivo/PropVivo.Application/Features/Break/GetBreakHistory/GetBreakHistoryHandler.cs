using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.Break;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Break;
using System.Net;

namespace PropVivo.Application.Features.Break.GetBreakHistory
{
    public class GetBreakHistoryHandler : IRequestHandler<GetBreakHistoryQuery, BaseResponse<List<BreakResponse>>>
    {
        private readonly IBreakRepository _breakRepository;
        private readonly IMapper _mapper;

        public GetBreakHistoryHandler(IBreakRepository breakRepository, IMapper mapper)
        {
            _breakRepository = breakRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<BreakResponse>>> Handle(GetBreakHistoryQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<BreakResponse>>();

            var breaks = await _breakRepository.GetByUserIdAsync(request.UserId);
            var filteredBreaks = breaks.Where(b => b.CreatedAt.Date >= request.StartDate.Date && b.CreatedAt.Date <= request.EndDate.Date).ToList();

            var breakResponses = new List<BreakResponse>();

            foreach (var breakItem in filteredBreaks)
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
            response.Message = "Break history retrieved successfully";

            return response;
        }
    }
} 