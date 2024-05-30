namespace SmartZonePhoneShop.DTO.ReviewDTOs
{
    public class UpdateReviewDTO
    {
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
