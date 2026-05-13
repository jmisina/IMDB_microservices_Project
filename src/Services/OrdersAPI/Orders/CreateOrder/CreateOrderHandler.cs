
using OrdersAPI.Data;
using OrdersAPI.DTO;
using OrdersAPI.Models;
using System.Net.Http.Json;

namespace OrdersAPI.Orders.CreateOrder
{
    public record CreateOrderCommand(int UserId, List<NewOrderItemCommand> OrderItems) : ICommand<CreateOrderResult>;

    public record CreateOrderResult(int Id, decimal TotalPrice);
    internal class CreateOrderHandler(DataContext context, IHttpClientFactory httpClientFactory) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("ProductsAPI");
            var products = new List<ProductDto>();

            foreach (var item in command.OrderItems)
            {
                var response = await client.GetAsync($"/products/{item.ProductId}", cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<GetProductByIdResponse>(cancellationToken: cancellationToken);
                    if (result?.Product != null)
                    {
                        products.Add(new ProductDto(result.Product.Id, result.Product.Name, result.Product.Price));
                    }
                }
                else
                {
                    throw new Exception($"Product with ID {item.ProductId} not found in ProductsAPI.");
                }
            }

            var order = new Order
            {
                UserId = command.UserId,
                OrderDate = DateTime.UtcNow,
                Status = "PENDING",

            };
            context.Orders.Add(order);

            await context.SaveChangesAsync(cancellationToken);

            var orderItems = command.OrderItems.Select(item =>
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {item.ProductId} not found.");
                }

                return new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                };
            }).ToList();

            context.OrderItems.AddRange(orderItems);

            var totalAmount = orderItems.Sum(item => item.UnitPrice * item.Quantity);

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = totalAmount,
                Status = "AWAITING",
            };

            context.Payments.Add(payment);

            await context.SaveChangesAsync(cancellationToken);

            return new CreateOrderResult(order.Id, totalAmount);

        }

        private record GetProductByIdResponse(ProductDto Product);
    }
}
