using System.ComponentModel.DataAnnotations;

namespace OrdersAPI.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; } 
        [Required]
        public DateTime OrderDate { get; set; }
        [MaxLength(20)]
        public string Status { get; set; } 

        // Navigation properties
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
