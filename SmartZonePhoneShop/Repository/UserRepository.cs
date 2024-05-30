using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.DTO.UserDTOs;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public bool UserExist(int userId)
        {
            return _context.Reviews.Any(r => r.ReviewId == userId);
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.OrderBy(r => r.UserId)
                            .Include(x => x.UserType)                          
                            .ToList();
        }



    }
}
