using OnlineShopBackend.Models;
using OnlineShopBackend.Repositories;

namespace OnlineShopBackend.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly ILogger<CustomerService> _log;

        public CustomerService(ICustomerRepository repo, ILogger<CustomerService> log)
        {
            _repo = repo;
            _log = log;
        }

        public Task AddAsync(CustomerDto customer)
        {
            _log.LogInformation("Adding customer {CustomerId}", customer.CustomerId);
            return _repo.AddAsync(customer);
        }
    }
}
