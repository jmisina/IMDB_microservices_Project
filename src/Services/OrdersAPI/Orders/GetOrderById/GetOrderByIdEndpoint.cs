
using OrdersAPI.DTO;
using OrdersAPI.Orders.CreateOrder;

namespace OrdersAPI.Orders.GetOrderById
{
    public record GetOrderByIdResponse(GetOrderResult Order);

    public class GetOrderByUserEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{id}", async (int id, ISender sender) =>
            {
                var result = await sender.Send(new GetOrderByIdQuery(id));

                var response = result.Adapt<GetOrderByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetOrderById")
            .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Orders")
            .WithDescription("Get Orders")
            .RequireAuthorization();
        }
    }
}

