
using OrdersAPI.DTO;
using OrdersAPI.Orders.CreateOrder;

namespace OrdersAPI.Orders.GetOrders
{
    public record GetOrdersResponse(IEnumerable<GetOrderResult> Orders);

    public class GetOrderByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async (ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery());

                var response = result.Adapt<GetOrdersResponse>();

                return Results.Ok(response);
            })
            .WithName("GetOrders")
            .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Orders")
            .WithDescription("Get Orders")
            .RequireAuthorization();
        }
    }
}

