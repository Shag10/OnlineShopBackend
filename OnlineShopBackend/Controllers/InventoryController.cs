using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;

namespace OnlineShopBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        public ActionResult SaveInventoryData()
        {
            SqlConnection sqlConnection = new SqlConnection
            {
                ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=OnlineShoppingDB;"
            };

            SqlCommand sqlCommand = new SqlCommand
            {
                Connection = sqlConnection,
                CommandText = "",
                CommandType = CommandType.StoredProcedure,
            };
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return Ok("Inventory saved successfully.");
        }
    }
}
