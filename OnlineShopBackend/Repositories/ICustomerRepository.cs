using OnlineShopBackend.Models;

namespace OnlineShopBackend.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task DeleteAsync(int customerId);
        Task UpdateAsync(Customer customer);
        Task<List<CustomerDto>> GetAsync(int? productId = null, int? page = null, int? pageSize = null);
    }
}
