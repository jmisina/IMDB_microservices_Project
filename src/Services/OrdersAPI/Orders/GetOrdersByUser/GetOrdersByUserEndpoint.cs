
using OrdersAPI.DTO;
using OrdersAPI.Orders.CreateOrder;

namespace OrdersAPI.Orders.GetOrdersByUser
{
    public record GetOrdersByUserResponse(List<GetOrderResult> Orders);

    public class GetOrdersByUserEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/user/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByUserQuery(id));

                var response = result.Adapt<GetOrdersByUserResponse>();

                return Results.Ok(response);
            })
            .WithName("GetOrderByUser")
            .Produces<GetOrdersByUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Orders by User Id")
            .WithDescription("Get Orders by User Id")
            .RequireAuthorization();
        }
    }
}

