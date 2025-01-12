using System.ComponentModel.DataAnnotations;

namespace OrdersAPI.DTO
{
    public class NewOrderItemCommand
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }

    }
}
