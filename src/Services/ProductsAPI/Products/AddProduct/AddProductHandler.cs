
namespace ProductsAPI.Products.AddProduct
{
    public record AddProductCommand(string Name, string Description,decimal Price, decimal Weight, int Stock, List<string> Category) : ICommand<AddProductResult>;

    public record AddProductResult(Guid Id);
    internal class AddProductCommandHandler (IDocumentSession session): ICommandHandler<AddProductCommand, AddProductResult>
    {
        public async Task<AddProductResult> Handle(AddProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Weight = command.Weight,
                Stock = command.Stock,
                Category = command.Category
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new AddProductResult(product.Id);
        }
    }
}
