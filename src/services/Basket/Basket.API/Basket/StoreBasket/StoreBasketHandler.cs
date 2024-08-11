using FluentValidation;

namespace Basket.API.Basket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(bool IsSuccess);

public class StoreBasketCommandValidatior : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidatior()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}


internal class StoreBasketHandler(IBasketRepository repository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {

        await repository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult(true);
    }
}
