using OnlineShopBackend.Models;

namespace OnlineShopBackend.Services
{
    public interface ICustomerService
    {
        Task AddAsync(CustomerDto customer);
        Task<List<CustomerDto>> GetAsync(int? customerId = null, int? page = null, int? pageSize = null);
    }
}
