
using OrdersAPI.DTO;
using OrdersAPI.Models;

namespace OrdersAPI.Orders.DeleteOrder
{
    public record DeleteOrderResponse(bool isSuccess);
    public class DeleteOrderEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{id}", async (int id, ISender sender) =>
            {

                var result = await sender.Send(new DeleteOrderCommand(id));

                var response = new DeleteOrderResponse(result.isSuccess);

                return Results.Ok(response);

            })
            .WithName("DeleteOrder")
            .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Order")
            .WithDescription("Delete Order")
            .RequireAuthorization();
        }

    }
}
