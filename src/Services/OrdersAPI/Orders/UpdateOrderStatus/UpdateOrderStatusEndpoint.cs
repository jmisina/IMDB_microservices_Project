
using OrdersAPI.DTO;
using OrdersAPI.Models;

namespace OrdersAPI.Orders.UpdateOrderStatus
{
    public record UpdateOrderStatusRequest(int OrderId);
    public record UpdateOrderStatusResponse(bool IsSuccess);
    public class CloseOrderEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/orders/{id}", async (int id, ISender sender) =>
            {

                var result = await sender.Send(new UpdateOrderStatusCommand(id));

                var response = result.Adapt<UpdateOrderStatusResponse>();

                return response;

            })
            .WithName("UpdateOrderStatus")
            .Produces<UpdateOrderStatusResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Order Status")
            .WithDescription("Update Order Status")
            .RequireAuthorization();
        }

    }
}
