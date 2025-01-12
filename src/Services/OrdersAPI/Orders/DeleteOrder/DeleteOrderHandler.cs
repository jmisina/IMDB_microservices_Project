
using OrdersAPI.Data;
using OrdersAPI.DTO;
using OrdersAPI.Models;

namespace OrdersAPI.Orders.DeleteOrder
{
    public record DeleteOrderCommand(int OrderId) : ICommand<DeleteOrderResult>;
    public record DeleteOrderResult(bool isSuccess);
    internal class UpdateOrderStatusHandler(DataContext context) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            Order? order = await context.Orders.SingleOrDefaultAsync(o => o.Id == command.OrderId);
            Payment? payment = await context.Payments.SingleOrDefaultAsync(u => u.OrderId == command.OrderId);
            List<OrderItem>? orderItems = await context.OrderItems
                .Where(oi => oi.OrderId == command.OrderId)
                .ToListAsync(cancellationToken);

            if (order == null | payment == null | orderItems == null)
            {
                return new DeleteOrderResult(false);
            }

            context.OrderItems.RemoveRange(orderItems);
            context.Payments.Remove(payment);
            context.Orders.Remove(order);


            await context.SaveChangesAsync(cancellationToken);

            return new DeleteOrderResult(true);

        }
    }
}
