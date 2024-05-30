using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SmartZonePhoneShop.Model
{
    public class UserType 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserTypeId { get; set; }
        [Required]
        public string Name { get; set; }

    }
}

