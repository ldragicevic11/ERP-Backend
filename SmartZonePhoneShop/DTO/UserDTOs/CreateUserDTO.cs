namespace SmartZonePhoneShop.DTO.UserDTOs
{
    public class CreateUserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int UserTypeId { get; set; }

    }
}

