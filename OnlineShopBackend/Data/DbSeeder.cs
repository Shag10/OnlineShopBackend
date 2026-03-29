using OnlineShopBackend.Models;

namespace OnlineShopBackend.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Inventories.Any()) return;

            var items = new List<Inventory>
            {
                new Inventory { ProductId = 1, ProductName = "HP Victus", StockQuantity = 25, ReorderStock = 10 },
                new Inventory { ProductId = 2, ProductName = "Dell Inspiron", StockQuantity = 15, ReorderStock = 5 },
                new Inventory { ProductId = 3, ProductName = "Lenovo ThinkPad", StockQuantity = 30, ReorderStock = 8 }
            };

            context.Inventories.AddRange(items);
            context.SaveChanges();
        }
    }
}
