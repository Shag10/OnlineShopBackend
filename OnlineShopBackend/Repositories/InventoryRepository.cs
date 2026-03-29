using Microsoft.EntityFrameworkCore;
using OnlineShopBackend.Data;
using OnlineShopBackend.Models;

namespace OnlineShopBackend.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _db;

        public InventoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Inventory inventory)
        {
            // If the DB has identity generation, you might ignore ProductId on add
            _db.Inventories.Add(inventory);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int productId)
        {
            var entity = await _db.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
            if (entity != null)
            {
                _db.Inventories.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<List<InventoryDto>> GetAsync(int? productId = null)
        {
            var query = _db.Inventories.AsQueryable();
            if (productId.HasValue)
                query = query.Where(i => i.ProductId == productId.Value);

            return await query.Select(i => new InventoryDto
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                StockQuantity = i.StockQuantity,
                ReorderStock = i.ReorderStock
            }).ToListAsync();
        }
    }
}
