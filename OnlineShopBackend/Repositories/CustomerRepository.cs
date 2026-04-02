using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineShopBackend.Data;
using OnlineShopBackend.Models;
using System.Data;

namespace OnlineShopBackend.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _db;

        public CustomerRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Customer customer)
        {
            var cs = _db.Database.GetDbConnection().ConnectionString;
            await using var conn = new SqlConnection(cs);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand("sp_SaveCustomerDetails", conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 30
            };

            cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerId);
            cmd.Parameters.AddWithValue("@CustomerName", customer.CustomerName ?? string.Empty);
            cmd.Parameters.AddWithValue("@Email", customer.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber ?? string.Empty);
            cmd.Parameters.AddWithValue("@Address", customer.Address ?? string.Empty);
            cmd.Parameters.AddWithValue("@RegistrationDate", customer.RegistrationDate);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int customerId)
        {
            var cs = _db.Database.GetDbConnection().ConnectionString;
            try
            {
                await using var conn = new SqlConnection(cs);
                await conn.OpenAsync();
                await using var cmd = new SqlCommand("sp_DeleteCustomerDetails", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 30
                };
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException)
            {
                // Fallback to EF Core delete if stored procedure is not available or fails
                var entity = await _db.Customers.FirstOrDefaultAsync(i => i.CustomerId == customerId);
                if (entity != null)
                {
                    _db.Customers.Remove(entity);
                    await _db.SaveChangesAsync();
                }
            }
        }

        public async Task<List<CustomerDto>> GetAsync(int? customerId = null, int? page = null, int? pageSize = null)
        {
            var results = new List<CustomerDto>();
            var cs = _db.Database.GetDbConnection().ConnectionString;
            await using var conn = new SqlConnection(cs);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand("sp_GetCustomerDetails", conn)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 60
            };
            if (customerId.HasValue)
                cmd.Parameters.AddWithValue("@CustomerId", customerId.Value);

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
                var dto = new CustomerDto
                {
                    CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                    CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    RegistrationDate = reader.GetDateTime(reader.GetOrdinal("RegistrationDate"))
                };
                results.Add(dto);
            }

            return results;
        }
    }
}
