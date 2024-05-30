using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Interface
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersByProductAsync(int productId);
        Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime fromDate, DateTime toDate);
        ICollection<Order> GetOrders();
        Order GetOrderByID(int id);
    }
}
