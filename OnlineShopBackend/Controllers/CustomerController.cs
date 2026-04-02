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

        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetInventoryData([FromQuery] int? customerId, [FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var list = await _service.GetAsync(customerId, page, pageSize);
            return Ok(list);
        }
    }
}
