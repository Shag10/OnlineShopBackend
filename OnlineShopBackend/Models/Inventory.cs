using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopBackend.Models
{
    [Table("Inventory")]
    public class Inventory
    {
        #region Properties

        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int StockQuantity { get; set; }
        public int ReorderStock { get; set; }

        #endregion
    }
}
