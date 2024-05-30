namespace SmartZonePhoneShop.DTO.CartItemDTOs
{
    public class CartItemDTO
    {
        public int Quantity { get; set; }
        public double SumPrice { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
    }
}
