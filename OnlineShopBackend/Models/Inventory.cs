namespace OnlineShopBackend.Models
{
    public class Inventory
    {
        #region Properties

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int StockQuantity { get; set; }
        public int ReorderStock { get; set; }

        #endregion
    }
}
