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

        public Task AddAsync(Customer customer)
        {
            _log.LogInformation("Adding customer {CustomerId}", customer.CustomerId);
            return _repo.AddAsync(customer);
        }

        public Task UpdateAsync(Customer customer)
        {
            _log.LogInformation("Updating customer {CustomerId}", customer.CustomerId);
            return _repo.UpdateAsync(customer);
        }

        public Task DeleteAsync(int customerId)
        {
            _log.LogInformation("Deleting customer {CustomerId}", customerId);
            return _repo.DeleteAsync(customerId);
        }

        public Task<List<CustomerDto>> GetAsync(int? customerId = null, int? page = null, int? pageSize = null)
        {
            _log.LogInformation("Getting customer list page={Page} size={Size}", page, pageSize);
            return _repo.GetAsync(customerId, page, pageSize);
        }
    }
}
