using FluentValidation;
using OnlineShopBackend.Models;

namespace OnlineShopBackend.Validators
{
    public class InventoryValidator : AbstractValidator<Inventory>
    {
        public InventoryValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.ProductName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.StockQuantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ReorderStock).GreaterThanOrEqualTo(0);
        }
    }
}
