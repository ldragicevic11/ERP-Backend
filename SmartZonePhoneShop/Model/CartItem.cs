using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartZonePhoneShop.Model
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double SumPrice { get; set; }
        
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
