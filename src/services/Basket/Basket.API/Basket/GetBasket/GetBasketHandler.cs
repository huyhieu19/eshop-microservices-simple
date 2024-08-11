
using Basket.API.Data;

namespace Basket.API.Basket;

public record GetBasketQuery(string userName) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart Cart);

internal class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await repository.GetBasket(request.userName, cancellationToken);

        return new GetBasketResult(basket);
    }
}
