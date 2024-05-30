using System.ComponentModel.DataAnnotations;

namespace SmartZonePhoneShop.Model
{
    public class RegisteredUser
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        

        
    }
}
