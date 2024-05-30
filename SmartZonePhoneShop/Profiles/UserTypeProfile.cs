using AutoMapper;
using SmartZonePhoneShop.DTO.UserTypeDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Profiles
{
    public class UserTypeProfile : Profile
    {
        public UserTypeProfile()
        {
            CreateMap<UserType, CreateUserTypeDTO>();
            CreateMap<CreateUserTypeDTO, UserType>();

            CreateMap<UserType, UpdateUserTypeDTO>();
            CreateMap<UpdateUserTypeDTO, UserType>();

        }
    }
}
