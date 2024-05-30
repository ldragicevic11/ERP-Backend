using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartZonePhoneShop.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _dbContext.Set<Order>()
                .Where(o => o.UserId == customerId)
                .ToListAsync();
        }


        public async Task<IEnumerable<Order>> GetOrdersByProductAsync(int productId)
        {
            return await _dbContext.Set<Order>()
                .Where(o => o.OrderItems.Any(oi => oi.ProductId == productId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime fromDate, DateTime toDate)
        {
            return await _dbContext.Set<Order>()
                .Where(o => o.Date >= fromDate && o.Date <= toDate)
                .ToListAsync();
        }

        public ICollection<Order> GetOrders()
        {
            return _context.Orders.OrderBy(r => r.OrderId)
                            .Include(x => x.User)
                            .Include(x => x.OrderStatus)
                            .Select(o => new Order
                            {
                                OrderId = o.OrderId,
                                Street = o.Street,
                                City = o.City,
                                Date = o.Date,
                                Quantity = o.Quantity,
                                PaymentSum = o.PaymentSum,
                                PaymentMethod = o.PaymentMethod,
                                UserId = o.UserId,
                                User = o.User,
                                OrderStatusId = o.OrderStatusId,
                                OrderStatus = o.OrderStatus,
                                OrderItems = o.OrderItems.Where(oi => oi is OrderItem).ToList()
                            })
                            .ToList();
        }



        public Order GetOrderByID(int id)
        {
            return _context.Orders.Where(r => r.OrderId == id)
                           .Include(x => x.User)
                           .Include(x => x.OrderStatus)
                           .Include(x => x.OrderItems)
                           .Select(o => new Order
                           {
                               OrderId = o.OrderId,
                               Street = o.Street,
                               City = o.City,
                               Date = o.Date,
                               Quantity = o.Quantity,
                               PaymentSum = o.PaymentSum,
                               PaymentMethod = o.PaymentMethod,
                               UserId = o.UserId,
                               User = o.User,
                               OrderStatusId = o.OrderStatusId,
                               OrderStatus = o.OrderStatus,
                               OrderItems = o.OrderItems.Where(oi => oi is OrderItem).ToList()
                           })
                           .FirstOrDefault();
        }


    }
}
