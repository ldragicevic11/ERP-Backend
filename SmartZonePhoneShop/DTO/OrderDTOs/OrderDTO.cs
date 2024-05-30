using SmartZonePhoneShop.DTO.OrderItemDTOs;

namespace SmartZonePhoneShop.DTO.OrderDTOs
{
    public class OrderDTO
    {
        public string Street { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public double PaymentSum { get; set; }
        public string PaymentMethod { get; set; }
        public int UserId { get; set; }
        public int OrderStatusName { get; set; }
        public ICollection<OrderItemDTO> OrderItems { get; set; }
    }
}
