using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.DTO.ReviewDTOs;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Repository
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }


        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.OrderBy(r => r.ReviewId)
                           .Include(x => x.User)
                            .Include(x => x.Product)
                           .ToList();
        }

        public ICollection<Review> GetReviewsByProductId(int productId)
        {
            return _context.Reviews
                .Where(x => x.ProductId == productId)
                .OrderBy(r => r.ReviewId)
                .Include(x => x.User)
                .Include(x => x.Product)
                .ToList();
        }

        //public Review GetReviewByID(int id)
        //{
        //    return _context.Reviews.Where(r => r.ReviewId == id)
        //                    .Include(x => x.User)
        //                    .Include(y => y.Product)
        //                    .FirstOrDefault();
        //}



        //public async Task<IEnumerable<Review>> GetReviewsByUserId(int userId) // Promenjeno u Task<IEnumerable<Review>>
        //{
        //    return await _context.Reviews.Where(r => r.UserId == userId)
        //        .Include(x => x.User)
        //        .Include(y => y.Product)
        //        .ToListAsync();
        //}

    }


}
