using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.Task;
using PropVivo.Domain.Enums;
using PropVivo.Infrastructure.Interfaces;
using Task = PropVivo.Domain.Entities.Task.Task;
using TaskStatus = PropVivo.Domain.Enums.TaskStatus;

namespace PropVivo.Infrastructure.Repositories
{
    public class TaskRepository : CosmosDbRepository<Task>, ITaskRepository
    {
        public TaskRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) 
            : base(cosmosDbContainerFactory)
        {
        }

        public override string ContainerName => "Tasks";

        public override string GenerateId(Task entity) => $"{Guid.NewGuid()}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public async Task<Task?> GetByIdAsync(string id)
        {
            return await GetItemAsync(id);
        }

        public async Task<List<Task>> GetAllAsync()
        {
            var request = new Request();
            var results = await GetItemsAsync(t => true, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<Task>> GetByAssignedToIdAsync(string assignedToId)
        {
            var request = new Request();
            var results = await GetItemsAsync(t => t.AssignedToId == assignedToId, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<Task>> GetByAssignedByIdAsync(string assignedById)
        {
            var request = new Request();
            var results = await GetItemsAsync(t => t.AssignedById == assignedById, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<Task>> GetByStatusAsync(TaskStatus status)
        {
            var request = new Request();
            var results = await GetItemsAsync(t => t.Status == status, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<Task>> GetActiveTasksByUserIdAsync(string userId)
        {
            var request = new Request();
            var results = await GetItemsAsync(t => t.AssignedToId == userId && 
                (t.Status == TaskStatus.Assigned || t.Status == TaskStatus.InProgress), request, x => x.Id);
            return results.ToList();
        }

        public async Task<Task> CreateAsync(Task task)
        {
            return await AddItemAsync(task);
        }

        public async Task<Task> UpdateAsync(Task task)
        {
            return await UpdateItemAsync(task.Id, task);
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

        public async Task<bool> HasActiveTaskAsync(string userId)
        {
            var activeTasks = await GetActiveTasksByUserIdAsync(userId);
            return activeTasks.Any();
        }

        public async Task<int> GetTaskCountByStatusAsync(string userId, TaskStatus status)
        {
            var request = new Request();
            var results = await GetItemsAsync(t => t.AssignedToId == userId && t.Status == status, request, x => x.Id);
            return results.Count();
        }
    }
}