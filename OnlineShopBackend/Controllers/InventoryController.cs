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

        [HttpPut("{productId?}")]
        public async Task<ActionResult> UpdateInventoryData([FromBody] Inventory inventoryDto)
        {
            try
            {
                await _service.UpdateAsync(inventoryDto);
                return Ok(new { message = "Product Details Updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Error updatig inventory data.", details = ex.Message });
            }
        }

        // Accept either DELETE /api/inventory/3 or DELETE /api/inventory?ProductId=3
        [HttpDelete("{productId?}")]
        public async Task<ActionResult> DeleteInventoryData([FromRoute] int? productId, [FromQuery(Name = "ProductId")] int? productIdQuery)
        {
            var id = productId ?? productIdQuery;
            if (!id.HasValue)
                return BadRequest(new { error = "ProductId is required either as route parameter or query string." });

            try
            {
                await _service.DeleteAsync(id.Value);
                return Ok(new { message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Error deleting inventory data.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<InventoryDto>>> GetInventoryData([FromQuery] int? productId)
        {
            var list = await _service.GetAsync(productId);
            return Ok(list);
        }
    }
}
