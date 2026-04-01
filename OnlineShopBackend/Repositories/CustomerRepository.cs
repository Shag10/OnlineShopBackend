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

        public async Task AddAsync(CustomerDto customer)
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
            cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
            cmd.Parameters.AddWithValue("@Address", customer.Address ?? string.Empty);
            cmd.Parameters.AddWithValue("@RegistrationDate", customer.RegistrationDate);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}
