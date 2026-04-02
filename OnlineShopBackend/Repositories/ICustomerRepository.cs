using OnlineShopBackend.Models;

namespace OnlineShopBackend.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(CustomerDto customer);
        Task<List<CustomerDto>> GetAsync(int? productId = null, int? page = null, int? pageSize = null);
    }
}
