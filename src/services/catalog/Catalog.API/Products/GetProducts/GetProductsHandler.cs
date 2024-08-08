namespace Catalog.API.Products;

public record GetProductsQuery() : IQuery<GetProductsResult>;
public record GetProductsResult(IEnumerable<Product> Products);

public class GetProductsQueryValidatior : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidatior()
    {
        //RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        //RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        //RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        //RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().ToListAsync(cancellationToken);
        return new GetProductsResult(products);
    }
}

