namespace SmartZonePhoneShop.DTO.ProductDTOs
{
    public class CreateProductDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public string Guarantee { get; set; }
        public int OnStock { get; set; }
    }
}
