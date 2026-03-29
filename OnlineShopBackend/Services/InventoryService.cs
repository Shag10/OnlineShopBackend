using OnlineShopBackend.Models;
using OnlineShopBackend.Repositories;

namespace OnlineShopBackend.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repo;

        public InventoryService(IInventoryRepository repo)
        {
            _repo = repo;
        }

        public Task AddAsync(Inventory inventory)
        {
            return _repo.AddAsync(inventory);
        }

        public Task DeleteAsync(int productId)
        {
            return _repo.DeleteAsync(productId);
        }

        public Task UpdateAsync(Inventory inventory)
        {
            return _repo.UpdateAsync(inventory);
        }

        public Task<List<InventoryDto>> GetAsync(int? productId = null)
        {
            return _repo.GetAsync(productId);
        }
    }
}
