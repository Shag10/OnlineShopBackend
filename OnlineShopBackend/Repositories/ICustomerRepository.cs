using OnlineShopBackend.Models;

namespace OnlineShopBackend.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(CustomerDto customer);
    }
}
