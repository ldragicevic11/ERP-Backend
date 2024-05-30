using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Repository
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public ICollection<OrderItem> GetOrderItems()
        {
            return _context.OrderItems.OrderBy(r => r.OrderItemId)
                           .Include(x => x.Order)
                            .Include(x => x.Product)
                           .ToList();
        }

        public OrderItem GetOrderItemByID(int id)
        {
            return _context.OrderItems.Where(r => r.OrderItemId == id)
                            .Include(x => x.Order)
                            .Include(y => y.Product)
                            .FirstOrDefault();
        }
    }
}
