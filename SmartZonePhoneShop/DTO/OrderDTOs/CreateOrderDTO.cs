using System.Collections.Generic;
using SmartZonePhoneShop.DTO;
using SmartZonePhoneShop.DTO.OrderItemDTOs;

namespace SmartZonePhoneShop.DTO.OrderDTOs
{
    public class CreateOrderDTO
    {
        public int OrderId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public double PaymentSum { get; set; }
        public string PaymentMethod { get; set; }
        public int UserId { get; set; }
        public int OrderStatusId { get; set; }
        
    }
}
