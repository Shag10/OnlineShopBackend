using OnlineShopBackend.Models;

namespace OnlineShopBackend.Services
{
    public interface ICustomerService
    {
        Task AddAsync(CustomerDto customer);
    }
}
