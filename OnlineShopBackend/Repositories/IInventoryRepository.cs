using OnlineShopBackend.Models;

namespace OnlineShopBackend.Repositories
{
    public interface IInventoryRepository
    {
        Task AddAsync(Inventory inventory);
        Task DeleteAsync(int productId);
        Task UpdateAsync(Inventory inventory);
        Task<List<InventoryDto>> GetAsync(int? productId = null, int? page = null, int? pageSize = null);
    }
}
