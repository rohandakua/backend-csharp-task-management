using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Repositories;
using PropVivo.Infrastructure.Interfaces;
using DomainUser = PropVivo.Domain.Entities.User.User;

namespace PropVivo.Infrastructure.Repositories
{
    public class UserRepository : CosmosDbRepository<DomainUser>, IUserRepository
    {
        public UserRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) 
            : base(cosmosDbContainerFactory)
        {
        }

        public override string ContainerName => "Users";

        public override string GenerateId(DomainUser entity) => $"{Guid.NewGuid()}";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public async Task<DomainUser?> GetByIdAsync(string id)
        {
            return await GetItemAsync(id);
        }

        public async Task<DomainUser?> GetByUsernameAsync(string username)
        {
            return await GetItemAsync(u => u.Username == username);
        }

        public async Task<DomainUser?> GetByEmailAsync(string email)
        {
            return await GetItemAsync(u => u.Email == email);
        }

        public async Task<List<DomainUser>> GetAllAsync()
        {
            var request = new Request();
            var results = await GetItemsAsync(u => true, request, x => x.Id);
            return results.ToList();
        }

        public async Task<List<DomainUser>> GetEmployeesBySuperiorIdAsync(string superiorId)
        {
            var request = new Request();
            var results = await GetItemsAsync(u => u.SuperiorId == superiorId, request, x => x.Id);
            return results.ToList();
        }

        public async Task<DomainUser> CreateAsync(DomainUser user)
        {
            return await AddItemAsync(user);
        }

        public async Task<DomainUser> UpdateAsync(DomainUser user)
        {
            return await UpdateItemAsync(user.Id, user);
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

        public async Task<bool> UsernameExistsAsync(string username)
        {
            var user = await GetByUsernameAsync(username);
            return user != null;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await GetByEmailAsync(email);
            return user != null;
        }
    }
}