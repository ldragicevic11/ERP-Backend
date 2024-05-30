using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Interface
{
    public interface ICartItemRepository : IBaseRepository<CartItem>
    {
        ICollection<CartItem> GetCartItems();
        CartItem GetCartItemByID(int id);
    }
}
