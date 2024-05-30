namespace SmartZonePhoneShop.DTO.ReviewDTOs
{
    public class CreateReviewDTO
    {
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
