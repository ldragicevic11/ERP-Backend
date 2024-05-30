using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartZonePhoneShop.Repository
{
    public class CartRepository : BaseRepository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<IEnumerable<Cart>> GetCartsByCustomerAsync(int userId)
        {
            return await _dbContext.Set<Cart>()
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cart>> GetCartsByProductAsync(int productId)
        {
            return await _dbContext.Set<Cart>()
                .Where(c => c.CartItems.Any(ci => ci.ProductId == productId))
                .ToListAsync();
        }

        public ICollection<Cart> GetCarts()
        {
            return _context.Carts.OrderBy(r => r.CartId)
                            .Include(x => x.User)
                            .Select(o => new Cart
                            {
                                CartId = o.CartId,
                                UserId = o.UserId,
                                CartItems = o.CartItems.Where(oi => oi is CartItem).ToList()
                            })
                            .ToList();
        }



        public Cart GetCartByID(int id)
        {
            return _context.Carts.Where(r => r.CartId == id)
                           .Include(x => x.User)
                           .Select(o => new Cart
                           {
                               CartId = o.CartId,
                               UserId = o.UserId,
                               CartItems = o.CartItems.Where(oi => oi is CartItem).ToList()
                           })
                           .FirstOrDefault();
        }

        public Cart GetCartWithCartItems(int id)
        {
            return _context.Carts.Where(r => r.CartId == id)
                           .Include(x => x.CartItems)
                           .ThenInclude(x => x.Product)
                           .Select(o => new Cart
                           {
                               CartId = o.CartId,
                               UserId = o.UserId,
                               CartItems = o.CartItems.Where(oi => oi is CartItem).ToList()
                           })
                           .FirstOrDefault();
        }

        public Cart? GetCartByUserID(int userId)
        {
            return _context.Carts.Where(r => r.UserId == userId)
                           .Include(x => x.User)
                           .Include(x => x.CartItems)
                           .ThenInclude(x => x.Product)
                           .Select(o => new Cart
                           {
                               CartId = o.CartId,
                               UserId = o.UserId,
                               CartItems = o.CartItems.Where(oi => oi is CartItem).ToList()
                           })
                           .FirstOrDefault();
        }
    }
}
