
namespace Catalog.API.Products;

// Cannot change name
public record GetProductsByCategoryQuery(string category) : IQuery<GetProductsByCategoryResult>;

public record GetProductsByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByCategoryQueryHandler.Handler called with {@Query}", query);

        var products = await session.Query<Product>().Where(p => p.Category.Contains(query.category)).ToListAsync(cancellationToken);

        return new GetProductsByCategoryResult(products);
    }
}
