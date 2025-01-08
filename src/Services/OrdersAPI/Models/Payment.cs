using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersAPI.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Order")]

        public int OrderId { get; set; } // Foreign key to Order
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } // e.g., "completed", "pending"

        // Navigation property
        public Order Order { get; set; }
    }
}
