using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Repository
{
    public class CartItemRepository : BaseRepository<CartItem>, ICartItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CartItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


        public ICollection<CartItem> GetCartItems()
        {
            return _context.CartItems.OrderBy(r => r.CartItemId)
                           .Include(x => x.Cart)
                            .Include(x => x.Product)
                           .ToList();
        }

        public CartItem GetCartItemByID(int id)
        {
            return _context.CartItems.Where(r => r.CartItemId == id)
                            .Include(x => x.Cart)
                            .Include(y => y.Product)
                            .FirstOrDefault();
        }

        
    }
}
