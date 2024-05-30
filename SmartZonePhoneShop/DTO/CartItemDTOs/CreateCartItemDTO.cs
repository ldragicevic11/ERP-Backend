namespace SmartZonePhoneShop.DTO.CartItemDTOs
{
    public class CreateCartItemDTO
    {
        public int Quantity { get; set; }
        public double SumPrice { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
