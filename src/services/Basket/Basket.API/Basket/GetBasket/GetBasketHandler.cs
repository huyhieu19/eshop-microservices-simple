
namespace Basket.API.Basket;

public record GetBasketQuery(string userName) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart Cart);

internal class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        // TODO: Get basket from database
        // var basket = =

        return new GetBasketResult(new ShoppingCart("john"));
    }
}
