using AutoMapper;
using SmartZonePhoneShop.DTO.OrderItemDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Profiles
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, CreateOrderItemDTO>();
            CreateMap<CreateOrderItemDTO, OrderItem>();

            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<OrderItemDTO, OrderItem>();

            CreateMap<OrderItem, UpdateOrderItemDTO>();
            CreateMap<UpdateOrderItemDTO, OrderItem>();
        }
    }
}
