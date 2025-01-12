using OrdersAPI.Data;
using OrdersAPI.DTO;

namespace OrdersAPI.Orders.GetOrders
{
    public record GetOrdersQuery() : ICommand<GetOrdersResult>;
    public record GetOrdersResult(IEnumerable<GetOrderResult> Orders);
    internal class GetOrderByIdHandler(DataContext context) : ICommandHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var orders = await (from o in context.Orders
                                join p in context.Payments on o.Id equals p.OrderId into paymentGroup
                                from payment in paymentGroup.DefaultIfEmpty()
                                select new
                                {
                                    Order = o,
                                    Payment = payment
                                })
                                .ToListAsync(cancellationToken);

            var orderIds = orders.Select(o => o.Order.Id).ToList();
            var orderItems = await context.OrderItems
                .Where(oi => orderIds.Contains(oi.OrderId))
                .ToListAsync(cancellationToken);

            var result = orders.Select(o => new GetOrderResult
            {
                Id = o.Order.Id,
                UserId = o.Order.UserId,
                OrderDate = o.Order.OrderDate,
                Status = o.Order.Status,
                PaymentData = o.Payment == null ? null : new PaymentInfo
                {
                    PaymentDate = o.Payment.PaymentDate,
                    Amount = o.Payment.Amount,
                    Status = o.Payment.Status
                },
                OrderItems = orderItems
                    .Where(oi => oi.OrderId == o.Order.Id)
                    .Select(oi => new ItemUnit
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    })
                    .ToList()
            }).ToList();

            return new GetOrdersResult(result);
        }
    }
}
