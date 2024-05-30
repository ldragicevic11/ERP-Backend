using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Repository
{
    public class UserTypeRepository : BaseRepository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
