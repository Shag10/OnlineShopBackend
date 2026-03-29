using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineShopBackend.Data;
using OnlineShopBackend.Models;
using System.Data.Common;

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
            var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_SaveInventoryData";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProductId", inventory.ProductId));
            cmd.Parameters.Add(new SqlParameter("@ProductName", inventory.ProductName ?? string.Empty));
            cmd.Parameters.Add(new SqlParameter("@StockQuantity", inventory.StockQuantity));
            cmd.Parameters.Add(new SqlParameter("@ReorderStock", inventory.ReorderStock));
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int productId)
        {
            var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_DeleteInventoryData";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@ProductId", productId));
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<InventoryDto>> GetAsync(int? productId = null)
        {
            var results = new List<InventoryDto>();
            var conn = _db.Database.GetDbConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "sp_GetInventoryData";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            if (productId.HasValue)
                cmd.Parameters.Add(new SqlParameter("@ProductId", productId.Value));

            using var reader = await cmd.ExecuteReaderAsync();
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
