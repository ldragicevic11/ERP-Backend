using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Interface
{
    public interface IOrderItemRepository : IBaseRepository<OrderItem>
    {
        ICollection<OrderItem> GetOrderItems();
        OrderItem GetOrderItemByID(int id);
    }
}
