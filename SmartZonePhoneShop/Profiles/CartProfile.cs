using AutoMapper;
using SmartZonePhoneShop.DTO.CartDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CreateCartDTO>();
            CreateMap<CreateCartDTO, Cart>();

            CreateMap<Cart, UpdateCartDTO>();
            CreateMap<UpdateCartDTO, Cart>();
        }
    }
}
