
using Marten.Pagination;

namespace ProductsAPI.Products.GetProducts
{
    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10, string? SearchTerm = null) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);

    internal class GetProductsQueryHandler(IDocumentSession session, ILogger<GetProductsQueryHandler> logger) 
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsQueryHandler.Handle called with {@Query}", query);

            var martenQuery = session.Query<Product>();

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                martenQuery = (Marten.Linq.IMartenQueryable<Product>)martenQuery.Where(p => p.Name.Contains(query.SearchTerm));
            }

            var products = await martenQuery.ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

            return new GetProductsResult(products);

        }
    }
}
