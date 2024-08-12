using FluentValidation;

namespace Basket.API.Basket;

public record DeleteBasketResult(bool IsSuccess);

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public class DeleteBasketComamndValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketComamndValidator()
    {
        RuleFor(p => p.UserName).NotEmpty().WithMessage("UserName is Require");
    }
}

internal class DeleteBasketHandler(IBasketRepository repository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteBasket(request.UserName, cancellationToken);

        return new DeleteBasketResult(true);
    }
}
