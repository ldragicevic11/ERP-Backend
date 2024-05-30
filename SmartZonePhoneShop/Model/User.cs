using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SmartZonePhoneShop.Model
{
    public class User 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public string Contact { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        [ForeignKey("UserType")]
        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }

        
    }

    
}
