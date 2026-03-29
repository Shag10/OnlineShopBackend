using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using OnlineShopBackend.Models;
using OnlineShopBackend.Services;

namespace OnlineShopBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> SaveInventoryData([FromBody] Inventory inventoryDto)
        {
            await _service.AddAsync(inventoryDto);
            return Ok(new { message = "Inventory saved successfully." });
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteInventoryData(int productId)
        {
            await _service.DeleteAsync(productId);
            return Ok(new { message = "Product Id deleted successfully." });
        }

        [HttpGet]
        public async Task<ActionResult<List<InventoryDto>>> GetInventoryData([FromQuery] int? productId)
        {
            var list = await _service.GetAsync(productId);
            return Ok(list);
        }
    }
}
