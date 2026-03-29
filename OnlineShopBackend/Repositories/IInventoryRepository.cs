using OnlineShopBackend.Models;

namespace OnlineShopBackend.Repositories
{
    public interface IInventoryRepository
    {
        Task AddAsync(Inventory inventory);
        Task DeleteAsync(int productId);
        Task<List<InventoryDto>> GetAsync(int? productId = null);
    }
}
