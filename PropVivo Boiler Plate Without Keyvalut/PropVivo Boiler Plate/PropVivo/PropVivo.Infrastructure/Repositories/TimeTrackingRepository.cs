using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.TimeTracking;
using PropVivo.Domain.Enums;
using PropVivo.Infrastructure.Interfaces;

namespace PropVivo.Infrastructure.Repositories
{
    public class TimeTrackingRepository : CosmosDbRepository<TimeTracking>, ITimeTrackingRepository
    {
        public TimeTrackingRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) 
            : base(cosmosDbContainerFactory)
        {
        }

        public override string ContainerName => "TimeTracking";

        public override string GenerateId(TimeTracking entity) => $"{Guid.NewGuid()}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public async Task<TimeTracking?> GetByIdAsync(string id)
        {
            return await GetItemAsync(id);
        }

        public async Task<List<TimeTracking>> GetAllAsync()
        {
            var request = new Request();
            var results = await GetItemsAsync(t => true, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<TimeTracking>> GetByUserIdAsync(string userId)
        {
            var request = new Request();
            var results = await GetItemsAsync(t => t.UserId == userId, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<TimeTracking>> GetByUserIdAndDateAsync(string userId, DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);
            var request = new Request();
            var results = await GetItemsAsync(t => t.UserId == userId && t.Date >= startDate && t.Date < endDate, request, x => x.Id);
            return results.ToList();
        }

        public async Task<TimeTracking?> GetActiveByUserIdAsync(string userId)
        {
            return await GetItemAsync(t => t.UserId == userId && t.Status == TimeTrackingStatus.Active);
        }

        public async Task<TimeTracking?> GetByUserIdAndTaskIdAsync(string userId, string taskId)
        {
            return await GetItemAsync(t => t.UserId == userId && t.TaskId == taskId);
        }

        public async Task<TimeTracking> CreateAsync(TimeTracking timeTracking)
        {
            return await AddItemAsync(timeTracking);
        }

        public async Task<TimeTracking> UpdateAsync(TimeTracking timeTracking)
        {
            return await UpdateItemAsync(timeTracking.Id, timeTracking);
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

        public async Task<bool> HasActiveTrackingAsync(string userId)
        {
            var activeTracking = await GetActiveByUserIdAsync(userId);
            return activeTracking != null;
        }

        public async Task<decimal> GetTotalHoursByUserIdAndDateAsync(string userId, DateTime date)
        {
            var timeTrackings = await GetByUserIdAndDateAsync(userId, date);
            return timeTrackings.Sum(t => t.TotalHours);
        }

        public async Task<decimal> GetTotalHoursByUserIdAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var timeTrackings = await GetByUserIdAndDateRangeAsync(userId, startDate, endDate);
            return timeTrackings.Sum(t => t.TotalHours);
        }

        public async Task<List<TimeTracking>> GetByUserIdAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var request = new Request();
            var results = await GetItemsAsync(t => t.UserId == userId && t.Date >= startDate && t.Date <= endDate, request, x => x.Id);
            return results.ToList();
        }
    }
}