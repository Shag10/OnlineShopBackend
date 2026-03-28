using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using OnlineShopBackend.Models;

namespace OnlineShopBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        [HttpPost]
        public ActionResult SaveInventoryData(Inventory inventoryDto)
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineShoppingDB;" +
                "Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;TrustServerCertificate=True;"
            };

            SqlCommand sqlCommand = new SqlCommand
            {
                Connection = sqlConnection,
                CommandText = "sp_SaveInventoryData",
                CommandType = CommandType.StoredProcedure,
            };

            sqlCommand.Parameters.AddWithValue("@ProductId", inventoryDto.ProductId);
            sqlCommand.Parameters.AddWithValue("@ProductName", inventoryDto.ProductName);
            sqlCommand.Parameters.AddWithValue("@StockQuantity", inventoryDto.StockQuantity);
            sqlCommand.Parameters.AddWithValue("@ReorderStock", inventoryDto.ReorderStock);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return Ok("Inventory saved successfully.");
        }
    }
}
