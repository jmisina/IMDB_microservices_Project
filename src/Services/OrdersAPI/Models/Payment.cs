using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersAPI.Models
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; } 
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } 

        public Order Order { get; set; }
    }
}
