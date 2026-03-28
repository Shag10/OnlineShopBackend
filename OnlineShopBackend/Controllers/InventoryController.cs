using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using OnlineShopBackend.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

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
            return new JsonResult(new { message = "Inventory saved successfully." });
        }

        [HttpDelete]
        public ActionResult DeleteInventoryData(int productId)
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineShoppingDB;" +
                "Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;TrustServerCertificate=True;"
            };

            SqlCommand sqlCommand = new SqlCommand
            {
                Connection = sqlConnection,
                CommandText = "sp_DeleteInventoryData",
                CommandType = CommandType.StoredProcedure,
            };
            sqlConnection.Open();
            sqlCommand.Parameters.AddWithValue("@ProductId", productId);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return new JsonResult(new { message = "Product Id deleted successfully." });
        }

        [HttpGet]
        public ActionResult GetInventoryData()
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineShoppingDB;" +
                "Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;TrustServerCertificate=True;"
            };

            SqlCommand sqlCommand = new SqlCommand
            {
                Connection = sqlConnection,
                CommandText = "sp_GetInventoryData",
                CommandType = CommandType.StoredProcedure,
            };
            sqlConnection.Open();
            List<InventoryDto> inventoryList = new List<InventoryDto>();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    InventoryDto inventory = new InventoryDto();
                    inventory.ProductId = reader.GetInt32(reader.GetOrdinal("ProductId"));
                    inventory.ProductName = reader.GetString(reader.GetOrdinal("ProductName"));
                    inventory.StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"));
                    inventory.ReorderStock = reader.GetInt32(reader.GetOrdinal("ReorderStock"));

                    inventoryList.Add(inventory);
                }
            }
            sqlConnection.Close();
            return Ok(JsonConvert.SerializeObject(inventoryList));
        }
    }
}
