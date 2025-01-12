using OrdersAPI.Data;
using OrdersAPI.DTO;

namespace OrdersAPI.Orders.GetOrderById
{
    public record GetOrderByIdQuery(int id) : ICommand<GetOrderByIdResult>;
    public record GetOrderByIdResult(GetOrderResult Order);
    internal class GetOrderByIdHandler(DataContext context) : ICommandHandler<GetOrderByIdQuery, GetOrderByIdResult>
    {
        public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
        {
            var orderWithPayment = await (from o in context.Orders
                                          join p in context.Payments on o.Id equals p.OrderId into paymentGroup
                                          from payment in paymentGroup.DefaultIfEmpty()
                                          where o.Id == query.id
                                          select new
                                          {
                                              Order = o,
                                              Payment = payment
                                          })
                                           .FirstOrDefaultAsync(cancellationToken);

            if (orderWithPayment == null)
            {
                throw new Exception("Order not found");
            }

            var orderItems = await context.OrderItems
                .Where(oi => oi.OrderId == query.id)
                .Select(oi => new ItemUnit
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                })
                .ToListAsync(cancellationToken);

            var result = new GetOrderResult
            {
                Id = orderWithPayment.Order.Id,
                UserId = orderWithPayment.Order.UserId,
                OrderDate = orderWithPayment.Order.OrderDate,
                Status = orderWithPayment.Order.Status,
                PaymentData = orderWithPayment.Payment == null ? null : new PaymentInfo
                {
                    PaymentDate = orderWithPayment.Payment.PaymentDate,
                    Amount = orderWithPayment.Payment.Amount,
                    Status = orderWithPayment.Payment.Status
                },
                OrderItems = orderItems
            };

            return new GetOrderByIdResult(result);
        }

    }
}
