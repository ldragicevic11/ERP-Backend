using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartZonePhoneShop.Model
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        
        public string Street { get; set; }
        
        public string City { get; set; }
        
        public DateTime Date { get; set; }

        public int Quantity { get; set; }

        public double PaymentSum { get; set; }
        [Required]
        public string PaymentMethod { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("OrderStatus")]
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
