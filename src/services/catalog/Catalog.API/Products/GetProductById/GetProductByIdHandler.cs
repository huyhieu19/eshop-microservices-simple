namespace Catalog.API.Products;

// Cannot change name
public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

public class GetProductByIdQueryValidatior : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidatior()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}


internal class GetProductByIdQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
        return product is null ? throw new ProductNotFoundException(query.Id) : new GetProductByIdResult(product);
    }
}
