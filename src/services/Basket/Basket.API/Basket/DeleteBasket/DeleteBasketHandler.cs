
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

public class DeleteBasketHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        return new DeleteBasketResult(true);
    }
}
