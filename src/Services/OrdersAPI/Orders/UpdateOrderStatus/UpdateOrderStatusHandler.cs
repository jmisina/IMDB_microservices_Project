
using Microsoft.EntityFrameworkCore;
using OrdersAPI.Data;
using OrdersAPI.DTO;
using OrdersAPI.Models;

namespace OrdersAPI.Orders.UpdateOrderStatus
{
    public record UpdateOrderStatusCommand(int OrderId) : ICommand<UpdateOrderStatusResult>;

    public record UpdateOrderStatusResult(bool IsSuccess);
    internal class CloseOrderHandler(DataContext context): ICommandHandler<UpdateOrderStatusCommand, UpdateOrderStatusResult>
    {
        public async Task<UpdateOrderStatusResult> Handle(UpdateOrderStatusCommand command, CancellationToken cancellationToken)
        {
            Order? order = await context.Orders.SingleOrDefaultAsync(o => o.Id == command.OrderId);
            Payment? payment = await context.Payments.SingleOrDefaultAsync(u => u.OrderId == command.OrderId);

            if (order == null | payment == null)
            {
                throw new Exception("Order not found!");
            }

            if (order.Status == "COMPLETED")
            {
                throw new Exception("This order is already completed!");
            }

            payment.PaymentDate = DateTime.UtcNow;
            payment.Status = "COMPLETED";
            order.Status = "COMPLETED";

            await context.SaveChangesAsync(cancellationToken);

            return new UpdateOrderStatusResult(true);

        }
    }
}
