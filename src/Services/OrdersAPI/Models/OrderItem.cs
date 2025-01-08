using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersAPI.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [Required]
        [MaxLength(50)]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }

        // Navigation property
        public Order Order { get; set; }
    }
}
