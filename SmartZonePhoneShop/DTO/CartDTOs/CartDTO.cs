using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.DTO.CartDTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
