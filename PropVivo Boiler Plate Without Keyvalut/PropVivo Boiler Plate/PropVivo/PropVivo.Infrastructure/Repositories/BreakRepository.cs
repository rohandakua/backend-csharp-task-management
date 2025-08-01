using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Break;
using PropVivo.Domain.Enums;
using PropVivo.Infrastructure.Interfaces;

namespace PropVivo.Infrastructure.Repositories
{
    public class BreakRepository : CosmosDbRepository<Break>, IBreakRepository
    {
        public BreakRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) 
            : base(cosmosDbContainerFactory)
        {
        }

        public override string ContainerName => "Breaks";

        public override string GenerateId(Break entity) => $"{Guid.NewGuid()}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public async Task<Break?> GetByIdAsync(string id)
        {
            return await GetItemAsync(id);
        }

        public async Task<List<Break>> GetAllAsync()
        {
            var request = new Request();
            var results = await GetItemsAsync(b => true, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<Break>> GetByUserIdAsync(string userId)
        {
            var request = new Request();
            var results = await GetItemsAsync(b => b.UserId == userId, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<Break>> GetByTimeTrackingIdAsync(string timeTrackingId)
        {
            var request = new Request();
            var results = await GetItemsAsync(b => b.TimeTrackingId == timeTrackingId, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<Break>> GetByUserIdAndDateAsync(string userId, DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);
            var request = new Request();
            var results = await GetItemsAsync(b => b.UserId == userId && 
                b.StartTime >= startDate && b.StartTime < endDate, request, x => x.Id);
            return results.ToList();
        }

        public async Task<Break?> GetActiveByUserIdAsync(string userId)
        {
            return await GetItemAsync(b => b.UserId == userId && b.EndTime == null);
        }

        public async Task<Break> CreateAsync(Break breakItem)
        {
            return await AddItemAsync(breakItem);
        }

        public async Task<Break> UpdateAsync(Break breakItem)
        {
            return await UpdateItemAsync(breakItem.Id, breakItem);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                await DeleteItemAsync(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ExistsAsync(string id)
        {
            var item = await GetItemAsync(id);
            return item != null;
        }

        public async Task<bool> HasActiveBreakAsync(string userId)
        {
            var activeBreak = await GetActiveByUserIdAsync(userId);
            return activeBreak != null;
        }

        public async Task<decimal> GetTotalBreakHoursByUserIdAndDateAsync(string userId, DateTime date)
        {
            var breaks = await GetByUserIdAndDateAsync(userId, date);
            return breaks.Sum(b => b.Duration);
        }
    }
}