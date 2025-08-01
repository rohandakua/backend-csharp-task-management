using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.TimeTracking;
using PropVivo.Domain.Enums;
using PropVivo.Infrastructure.Interfaces;
using Microsoft.Azure.Cosmos;

namespace PropVivo.Infrastructure.Repositories
{
    public class TimeTrackingRepository : CosmosDbRepository<TimeTracking>, ITimeTrackingRepository
    {
        public override string ContainerName => "timetracking";

        public TimeTrackingRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) 
            : base(cosmosDbContainerFactory)
        {
        }

        public override string GenerateId(TimeTracking entity)
        {
            return Guid.NewGuid().ToString();
        }

        public override PartitionKey ResolvePartitionKey(string entityId)
        {
            return new PartitionKey(entityId);
        }

        public async Task<TimeTracking?> GetByIdAsync(string id)
        {
            try
            {
                return await GetItemAsync(id, id);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<TimeTracking>> GetAllAsync()
        {
            var results = await GetItemsAsync("SELECT * FROM c");
            return results.ToList();
        }

        public async Task<List<TimeTracking>> GetByUserIdAsync(string userId)
        {
            var results = await GetItemsAsync($"SELECT * FROM c WHERE c.userId = '{userId}'");
            return results.ToList();
        }

        public async Task<List<TimeTracking>> GetByUserIdAndDateAsync(string userId, DateTime date)
        {
            var results = await GetItemsAsync($"SELECT * FROM c WHERE c.userId = '{userId}' AND c.date = '{date:yyyy-MM-dd}'");
            return results.ToList();
        }

        public async Task<List<TimeTracking>> GetByUserIdAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var results = await GetItemsAsync($"SELECT * FROM c WHERE c.userId = '{userId}' AND c.date >= '{startDate:yyyy-MM-dd}' AND c.date <= '{endDate:yyyy-MM-dd}'");
            return results.ToList();
        }

        public async Task<TimeTracking?> GetActiveByUserIdAsync(string userId)
        {
            var results = await GetItemsAsync($"SELECT * FROM c WHERE c.userId = '{userId}' AND c.status = {(int)TimeTrackingStatus.Active}");
            return results.FirstOrDefault();
        }

        public async Task<TimeTracking?> GetByUserIdAndTaskIdAsync(string userId, string taskId)
        {
            var results = await GetItemsAsync($"SELECT * FROM c WHERE c.userId = '{userId}' AND c.taskId = '{taskId}'");
            return results.FirstOrDefault();
        }

        public async Task<TimeTracking> CreateAsync(TimeTracking timeTracking)
        {
            timeTracking.Id = Guid.NewGuid().ToString();
            return await AddItemAsync(timeTracking, timeTracking.UserId);
        }

        public async Task<TimeTracking> UpdateAsync(TimeTracking timeTracking)
        {
            timeTracking.UpdatedAt = DateTime.UtcNow;
            return await UpdateItemAsync(timeTracking.Id, timeTracking, timeTracking.UserId);
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
            var item = await GetByIdAsync(id);
            return item != null;
        }

        public async Task<bool> HasActiveTrackingAsync(string userId)
        {
            var activeTracking = await GetActiveByUserIdAsync(userId);
            return activeTracking != null;
        }

        public async Task<decimal> GetTotalHoursByUserIdAndDateAsync(string userId, DateTime date)
        {
            var timeTrackingRecords = await GetByUserIdAndDateAsync(userId, date);
            return timeTrackingRecords.Sum(t => t.TotalHours);
        }

        public async Task<decimal> GetTotalHoursByUserIdAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            var timeTrackingRecords = await GetByUserIdAndDateRangeAsync(userId, startDate, endDate);
            return timeTrackingRecords.Sum(t => t.TotalHours);
        }
    }
}