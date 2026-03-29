using OnlineShopBackend.Models;

namespace OnlineShopBackend.Services
{
    public interface IInventoryService
    {
        Task AddAsync(Inventory inventory);
        Task DeleteAsync(int productId);
        Task<List<InventoryDto>> GetAsync(int? productId = null);
    }
}
