using OrdersAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace OrdersAPI.DTO
{
    public class ItemUnit 
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
    public class PaymentInfo 
    {
        public DateTime? PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }

    }
    public class GetOrderResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }        
        public PaymentInfo PaymentData { get; set; }

        public ICollection<ItemUnit> OrderItems { get; set; } = new List<ItemUnit>();

    }
}
