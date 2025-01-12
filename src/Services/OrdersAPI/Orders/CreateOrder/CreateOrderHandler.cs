
using OrdersAPI.Data;
using OrdersAPI.DTO;
using OrdersAPI.Models;

namespace OrdersAPI.Orders.CreateOrder
{
    public record CreateOrderCommand(int UserId, List<NewOrderItemCommand> OrderItems) : ICommand<CreateOrderResult>;

    public record CreateOrderResult(int Id);
    internal class CreateOrderHandler(DataContext context): ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                UserId = command.UserId,
                OrderDate = DateTime.UtcNow,
                Status = "PENDING",

            };
            context.Orders.Add(order);

            await context.SaveChangesAsync(cancellationToken);

            var orderItems = command.OrderItems.Select(item => new OrderItem
            {
                OrderId = order.Id, 
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
            }).ToList();

            context.OrderItems.AddRange(orderItems);

            var totalAmount = orderItems
            .Sum(item => command.OrderItems.First(c => c.ProductId == item.ProductId).UnitPrice * item.Quantity);

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = totalAmount,
                Status = "AWAITING",
            };

            context.Payments.Add(payment);

            await context.SaveChangesAsync(cancellationToken);

            return new CreateOrderResult(order.Id);

        }
    }
}
