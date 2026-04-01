using Microsoft.AspNetCore.Mvc;
using OnlineShopBackend.Models;
using OnlineShopBackend.Services;

namespace OnlineShopBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> SaveCustomerData([FromBody] CustomerDto customerDto)
        {
            await _service.AddAsync(customerDto);
            return Ok(new { message = "Customer Details saved successfully." });
        }
    }
}
