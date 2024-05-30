using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Interface
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        ICollection<Review> GetReviews();
        ICollection<Review> GetReviewsByProductId(int productId);
        //Review GetReviewByID(int id);
        //IEnumerable<Review> GetReviewsByUserId(int userId);


    }
}
