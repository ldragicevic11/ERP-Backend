using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Interface
{
    public interface ICartRepository : IBaseRepository<Cart>
    {
        Task<IEnumerable<Cart>> GetCartsByCustomerAsync(int userId);
        Task<IEnumerable<Cart>> GetCartsByProductAsync(int productId);
        ICollection<Cart> GetCarts();
        Cart GetCartByID(int id);
        Cart GetCartWithCartItems(int id);
        Cart? GetCartByUserID(int userId);
    }
}
