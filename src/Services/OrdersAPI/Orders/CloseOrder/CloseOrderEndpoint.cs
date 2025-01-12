
using OrdersAPI.DTO;
using OrdersAPI.Models;

namespace OrdersAPI.Orders.CloseOrder
{
    public record CloseOrderRequest(int OrderId);
    public record CloseOrderResponse(bool IsSuccess);
    public class CloseOrderEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/orders/{id}/close", async (int id, ISender sender) =>
            {

                var result = await sender.Send(new CloseOrderCommand(id));

                var response = result.Adapt<CloseOrderResponse>();

                return response;

            })
            .WithName("CloseOrder")
            .Produces<CloseOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Close Order")
            .WithDescription("Close Order")
            .RequireAuthorization();
        }

    }
}
