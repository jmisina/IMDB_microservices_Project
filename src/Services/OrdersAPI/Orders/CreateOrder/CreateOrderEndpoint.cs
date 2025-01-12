
using OrdersAPI.DTO;
using OrdersAPI.Models;

namespace OrdersAPI.Orders.CreateOrder
{
    public record CreateOrderRequest(int UserId, List<NewOrderItemCommand> OrderItems);
    
    public record CreateOrderResponse(int Id);
    public class CreateOrderEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOrderCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateOrderResponse>();

                return Results.Ok(response);

            })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Create Order")
            .WithDescription("Create Order")
            .RequireAuthorization();
        }

    }
}
