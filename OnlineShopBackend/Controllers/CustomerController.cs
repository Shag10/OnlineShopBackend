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
        public async Task<ActionResult> SaveCustomerData([FromBody] Customer customer)
        {
            await _service.AddAsync(customer);
            return Ok(new { message = "Customer Details saved successfully." });
        }

        [HttpDelete("{customerId?}")]
        public async Task<ActionResult> DeleteCustomerData([FromRoute] int? customerId, [FromQuery(Name = "ProductId")] int? customerIdQuery)
        {
            var id = customerId ?? customerIdQuery;
            if (!id.HasValue)
                return BadRequest(new { error = "CustomerId is required either as route parameter or query string." });

            try
            {
                await _service.DeleteAsync(id.Value);
                return Ok(new { message = "Customer Detail deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Error deleting inventory data.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetCustomerData([FromQuery] int? customerId, [FromQuery] int? page, [FromQuery] int? pageSize)
        {
            var list = await _service.GetAsync(customerId, page, pageSize);
            return Ok(list);
        }
    }
}
