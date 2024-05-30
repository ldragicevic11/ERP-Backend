using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartZonePhoneShop.Model
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }
        [Required]
        public double ProductPrice { get; set; }
        [Required]
        public int Quantity { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
