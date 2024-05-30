using AutoMapper;
using SmartZonePhoneShop.DTO.UserDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, CreateUserDTO>();
            CreateMap<CreateUserDTO, User>();

            CreateMap<User, RegisterUserDTO>();
            CreateMap<RegisterUserDTO, User>();

            CreateMap<User, UpdateUserDTO>();
            CreateMap<UpdateUserDTO, User>();

            CreateMap<User, UserLoginDTO>();
            CreateMap<UserLoginDTO, User>();

            CreateMap<User, UserReviewDTO>();
            CreateMap<UserReviewDTO, User>();

            CreateMap<User, UserCheckDTO>();
            CreateMap<UserCheckDTO, User>();

        }
    }
}
