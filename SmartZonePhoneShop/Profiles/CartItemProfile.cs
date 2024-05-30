using AutoMapper;
using SmartZonePhoneShop.DTO.CartItemDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Profiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemDTO>();
            CreateMap<CartItemDTO, CartItem>();

            CreateMap<CartItem, CreateCartItemDTO>();
            CreateMap<CreateCartItemDTO, CartItem>();

            CreateMap<CartItem, UpdateCartItemDTO>();
            CreateMap<UpdateCartItemDTO, CartItem>();
        }
    }
}
