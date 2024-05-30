using AutoMapper;
using SmartZonePhoneShop.DTO.OrderDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, CreateOrderDTO>();
            CreateMap<CreateOrderDTO, Order>();

            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();

            CreateMap<Order, UpdateOrderDTO>();
            CreateMap<UpdateOrderDTO, Order>();

        }
    }
}
