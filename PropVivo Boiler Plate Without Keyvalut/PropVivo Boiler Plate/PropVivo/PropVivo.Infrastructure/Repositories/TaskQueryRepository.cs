using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.TaskQuery;
using PropVivo.Domain.Enums;
using PropVivo.Infrastructure.Interfaces;

namespace PropVivo.Infrastructure.Repositories
{
    public class TaskQueryRepository : CosmosDbRepository<TaskQuery>, ITaskQueryRepository
    {
        public TaskQueryRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) 
            : base(cosmosDbContainerFactory)
        {
        }

        public override string ContainerName => "TaskQueries";

        public override string GenerateId(TaskQuery entity) => $"{Guid.NewGuid()}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public async Task<TaskQuery?> GetByIdAsync(string id)
        {
            return await GetItemAsync(id);
        }

        public async Task<List<TaskQuery>> GetAllAsync()
        {
            var request = new Request();
            var results = await GetItemsAsync(tq => true, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<TaskQuery>> GetByTaskIdAsync(string taskId)
        {
            var request = new Request();
            var results = await GetItemsAsync(tq => tq.TaskId == taskId, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<TaskQuery>> GetByRaisedByIdAsync(string raisedById)
        {
            var request = new Request();
            var results = await GetItemsAsync(tq => tq.RaisedById == raisedById, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<TaskQuery>> GetByAssignedToIdAsync(string assignedToId)
        {
            var request = new Request();
            var results = await GetItemsAsync(tq => tq.AssignedToId == assignedToId, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<TaskQuery>> GetByStatusAsync(QueryStatus status)
        {
            var request = new Request();
            var results = await GetItemsAsync(tq => tq.Status == status, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<TaskQuery>> GetByPriorityAsync(QueryPriority priority)
        {
            var request = new Request();
            var results = await GetItemsAsync(tq => tq.Priority == priority, request, x => x.Id);
            return results.ToList();
        }

        public async Task<TaskQuery> CreateAsync(TaskQuery taskQuery)
        {
            return await AddItemAsync(taskQuery);
        }

        public async Task<TaskQuery> UpdateAsync(TaskQuery taskQuery)
        {
            return await UpdateItemAsync(taskQuery.Id, taskQuery);
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

        public async Task<int> GetQueryCountByStatusAsync(string userId, QueryStatus status)
        {
            var request = new Request();
            var results = await GetItemsAsync(tq => tq.RaisedById == userId && tq.Status == status, request, x => x.Id);
            return results.Count();
        }
    }
}