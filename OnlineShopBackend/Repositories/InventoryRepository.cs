using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineShopBackend.Data;
using OnlineShopBackend.Models;
using System.Data.Common;
using System.Data;

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
            var cs = _db.Database.GetDbConnection().ConnectionString;
            await using var conn = new SqlConnection(cs);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand("sp_SaveInventoryData", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@ProductId", inventory.ProductId);
            cmd.Parameters.AddWithValue("@ProductName", inventory.ProductName ?? string.Empty);
            cmd.Parameters.AddWithValue("@StockQuantity", inventory.StockQuantity);
            cmd.Parameters.AddWithValue("@ReorderStock", inventory.ReorderStock);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int productId)
        {
            var cs = _db.Database.GetDbConnection().ConnectionString;
            try
            {
                await using var conn = new SqlConnection(cs);
                await conn.OpenAsync();
                await using var cmd = new SqlCommand("sp_DeleteInventoryData", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ProductId", productId);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException)
            {
                // Fallback to EF Core delete if stored procedure is not available or fails
                var entity = await _db.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId);
                if (entity != null)
                {
                    _db.Inventories.Remove(entity);
                    await _db.SaveChangesAsync();
                }
            }
        }

        public async Task<List<InventoryDto>> GetAsync(int? productId = null)
        {
            var results = new List<InventoryDto>();
            var cs = _db.Database.GetDbConnection().ConnectionString;
            await using var conn = new SqlConnection(cs);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand("sp_GetInventoryData", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (productId.HasValue)
                cmd.Parameters.AddWithValue("@ProductId", productId.Value);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var dto = new InventoryDto
                {
                    ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                    ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                    StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                    ReorderStock = reader.GetInt32(reader.GetOrdinal("ReorderStock"))
                };
                results.Add(dto);
            }

            return results;
        }
    }
}
