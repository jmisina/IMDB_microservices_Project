
namespace ProductsAPI.Products.AddProduct
{
    public record AddProductRequest(string Name, string Description, decimal Price, decimal Weight, int Stock, List<string> Category);
    
    public record AddProductResponse(Guid Id);
    public class AddProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (AddProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<AddProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<AddProductResponse>();

                return Results.Created($"/products/{response.Id}", response);

            })
            .WithName("AddProduct")
            .Produces<AddProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Add Product")
            .WithDescription("Add Product");
        }

    }
}
