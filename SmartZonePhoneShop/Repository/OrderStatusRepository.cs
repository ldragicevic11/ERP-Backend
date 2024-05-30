using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Repository
{
    public class OrderStatusRepository : BaseRepository<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}