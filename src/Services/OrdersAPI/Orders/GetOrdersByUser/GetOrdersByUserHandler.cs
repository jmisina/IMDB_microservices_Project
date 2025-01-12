using OrdersAPI.Data;
using OrdersAPI.DTO;

namespace OrdersAPI.Orders.GetOrdersByUser
{
    public record GetOrdersByUserQuery(int id) : ICommand<GetOrdersByUserResult>;
    public record GetOrdersByUserResult(List<GetOrderResult> Orders);
    internal class GetOrdersByUserHandler(DataContext context) : ICommandHandler<GetOrdersByUserQuery, GetOrdersByUserResult>
    {
        public async Task<GetOrdersByUserResult> Handle(GetOrdersByUserQuery query, CancellationToken cancellationToken)
        {
            // Step 1: Fetch orders and their related payments for the specified UserId
            var ordersWithPayments = await (from o in context.Orders
                                            join p in context.Payments on o.Id equals p.OrderId into paymentGroup
                                            from payment in paymentGroup.DefaultIfEmpty()
                                            where o.UserId == query.id
                                            select new
                                            {
                                                Order = o,
                                                Payment = payment
                                            })
                                            .ToListAsync(cancellationToken);

            // Ensure orders exist for the given UserId
            if (!ordersWithPayments.Any())
            {
                return new GetOrdersByUserResult(new List<GetOrderResult>()); // Return empty result instead of null
            }

            // Step 2: Extract all OrderIds for this user
            var orderIds = ordersWithPayments.Select(op => op.Order.Id).ToList();

            // Step 3: Fetch all OrderItems that belong to the user's orders
            var orderItems = await context.OrderItems
                .Where(oi => orderIds.Contains(oi.OrderId)) // Filter by OrderIds related to the UserId
                .Select(oi => new
                {
                    oi.OrderId,
                    Item = new ItemUnit
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }
                })
                .ToListAsync(cancellationToken);

            // Step 4: Map results into the desired format
            var result = ordersWithPayments.Select(op => new GetOrderResult
            {
                Id = op.Order.Id,
                UserId = op.Order.UserId,
                OrderDate = op.Order.OrderDate,
                Status = op.Order.Status,
                PaymentData = op.Payment == null ? null : new PaymentInfo
                {
                    PaymentDate = op.Payment.PaymentDate,
                    Amount = op.Payment.Amount,
                    Status = op.Payment.Status
                },
                OrderItems = orderItems
                    .Where(oi => oi.OrderId == op.Order.Id) // Ensure correct OrderItems for each Order
                    .Select(oi => oi.Item)
                    .ToList()
            }).ToList();

            return new GetOrdersByUserResult(result);
        }

    }

}

