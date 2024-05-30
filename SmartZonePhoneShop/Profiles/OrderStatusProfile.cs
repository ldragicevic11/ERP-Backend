using AutoMapper;
using SmartZonePhoneShop.DTO.OrderStatusDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Profiles
{
    public class OrderStatusProfile : Profile
    {
        public OrderStatusProfile()
        {
            CreateMap<OrderStatus, OrderStatusDTO>();
            CreateMap<OrderStatusDTO, OrderStatus>();
        }
    }
}
