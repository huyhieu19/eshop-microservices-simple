using Marten.Pagination;

namespace Catalog.API.Products;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

public class GetProductsQueryValidatior : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidatior()
    {
        RuleFor(x => x.PageNumber).LessThanOrEqualTo(0).WithMessage("Price must be less than 0");
        RuleFor(x => x.PageSize).LessThanOrEqualTo(0).WithMessage("Price must be less than 0");
    }
}

internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        //var predicates = new List<Expression<Func<Product, object>>>
        //{
        //    // List properties must check
        //    p => p.Name,
        //    p => p.Description,
        //    p => p.Category,
        //    p => p.Price,
        //};
        //var predicate = SearchExtensions.CreateSearchPredicate(query.searchTerm ?? "", predicates);
        var products = await session.Query<Product>()/*.Where(predicate)*/.ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);
        return new GetProductsResult(products);
    }
}

