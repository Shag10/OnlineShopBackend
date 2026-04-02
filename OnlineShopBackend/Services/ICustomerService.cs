using OnlineShopBackend.Models;

namespace OnlineShopBackend.Services
{
    public interface ICustomerService
    {
        Task AddAsync(Customer customer);
        Task DeleteAsync(int customerId);
        Task UpdateAsync(Customer customer);
        Task<List<CustomerDto>> GetAsync(int? customerId = null, int? page = null, int? pageSize = null);
    }
}
