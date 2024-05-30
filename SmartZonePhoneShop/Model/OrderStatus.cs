using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartZonePhoneShop.Model
{
    public class OrderStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderStatusId { get; set; }
        [Required]
        public string StatusName { get; set; }
    }
}
