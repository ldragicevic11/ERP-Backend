using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartZonePhoneShop.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public double Price { get; set; }
        
        public string Guarantee { get; set; }
        [Required]
        public int OnStock { get; set; }
    }
}
