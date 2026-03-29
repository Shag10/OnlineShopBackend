using Microsoft.Extensions.Logging;
using OnlineShopBackend.Models;
using OnlineShopBackend.Repositories;

namespace OnlineShopBackend.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repo;
        private readonly ILogger<InventoryService> _log;

        public InventoryService(IInventoryRepository repo, ILogger<InventoryService> log)
        {
            _repo = repo;
            _log = log;
        }

        public Task AddAsync(Inventory inventory)
        {
            _log.LogInformation("Adding inventory {ProductId}", inventory.ProductId);
            return _repo.AddAsync(inventory);
        }

        public Task DeleteAsync(int productId)
        {
            _log.LogInformation("Deleting inventory {ProductId}", productId);
            return _repo.DeleteAsync(productId);
        }

        public Task UpdateAsync(Inventory inventory)
        {
            _log.LogInformation("Updating inventory {ProductId}", inventory.ProductId);
            return _repo.UpdateAsync(inventory);
        }

        public Task<List<InventoryDto>> GetAsync(int? productId = null, int? page = null, int? pageSize = null)
        {
            _log.LogInformation("Getting inventory list page={Page} size={Size}", page, pageSize);
            return _repo.GetAsync(productId, page, pageSize);
        }
    }
}
