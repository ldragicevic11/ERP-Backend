using SmartZonePhoneShop.DTO.UserDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Interface
{
    public interface IUserRepository : IBaseRepository<User>
    {
        bool UserExist(int userID);

        ICollection<User> GetUsers();

    }
}
