using MediatR;

namespace ProductsAPI.Products.AddProduct
{
    public record AddProductCommand(string Name, string Description,decimal Price, decimal Weight, int Stock, List<string> Category) : IRequest<AddProductResult>;

    public record AddProductResult(Guid Id);
    internal class AddProductCommandHandler : IRequestHandler<AddProductCommand, AddProductResult>
    {
        public Task<AddProductResult> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
