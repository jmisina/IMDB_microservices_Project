
using Microsoft.EntityFrameworkCore;
using OrdersAPI.Data;
using OrdersAPI.DTO;
using OrdersAPI.Models;

namespace OrdersAPI.Orders.CloseOrder
{
    public record CloseOrderCommand(int OrderId) : ICommand<CloseOrderResult>;

    public record CloseOrderResult(bool IsSuccess);
    internal class CloseOrderHandler(DataContext context): ICommandHandler<CloseOrderCommand, CloseOrderResult>
    {
        public async Task<CloseOrderResult> Handle(CloseOrderCommand command, CancellationToken cancellationToken)
        {
            Order? order = await context.Orders.SingleOrDefaultAsync(o => o.Id == command.OrderId);

            if (order == null)
            {
                throw new Exception("Order not found!");
            }

            if (order.Status == "COMPLETED")
            {
                throw new Exception("This order is already completed!");
            }

            order.Status = "COMPLETED";

            await context.SaveChangesAsync(cancellationToken);

            return new CloseOrderResult(true);

        }
    }
}
