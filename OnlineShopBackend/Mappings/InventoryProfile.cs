using AutoMapper;
using OnlineShopBackend.Models;

namespace OnlineShopBackend.Mappings
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventory, InventoryDto>().ReverseMap();
        }
    }
}
