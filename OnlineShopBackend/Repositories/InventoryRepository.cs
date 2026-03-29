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
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 30
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
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 30
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

        public async Task UpdateAsync(Inventory inventory)
        {
            var cs = _db.Database.GetDbConnection().ConnectionString;
            try
            {
                await using var conn = new SqlConnection(cs);
                await conn.OpenAsync();
                await using var cmd = new SqlCommand("sp_UpdateInventoryData", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 30
                };
                cmd.Parameters.AddWithValue("@ProductId", inventory.ProductId);
                cmd.Parameters.AddWithValue("@ProductName", inventory.ProductName ?? string.Empty);
                cmd.Parameters.AddWithValue("@StockQuantity", inventory.StockQuantity);
                cmd.Parameters.AddWithValue("@ReorderStock", inventory.ReorderStock);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException)
            {
                var entity = await _db.Inventories.FirstOrDefaultAsync(i => i.ProductId == inventory.ProductId);
                if (entity != null)
                {
                    _db.Inventories.Update(entity);
                    await _db.SaveChangesAsync();
                }
            }
        }

        public async Task<List<InventoryDto>> GetAsync(int? productId = null, int? page = null, int? pageSize = null)
        {
            var results = new List<InventoryDto>();
            var cs = _db.Database.GetDbConnection().ConnectionString;
            await using var conn = new SqlConnection(cs);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand("sp_GetInventoryData", conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 60
            };
            if (productId.HasValue)
                cmd.Parameters.AddWithValue("@ProductId", productId.Value);

            // If pagination parameters were provided, add them as parameters for the stored procedure
            // Stored proc can optionally handle @Page and @PageSize to support pagination server-side
            if (page.HasValue)
                cmd.Parameters.AddWithValue("@Page", page.Value);
            if (pageSize.HasValue)
                cmd.Parameters.AddWithValue("@PageSize", pageSize.Value);

            // Use SequentialAccess and CloseConnection so the reader streams rows and closes the connection when disposed
            await using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection);
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
