namespace SmartZonePhoneShop.DTO.OrderItemDTOs
{
    public class CreateOrderItemDTO
    {
        public int OrderItemId { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
