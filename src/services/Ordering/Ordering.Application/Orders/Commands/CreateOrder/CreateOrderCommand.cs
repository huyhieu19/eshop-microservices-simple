using FluentValidation;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public record CreateOrderResult(Guid Id);

public record CreateOrderCommand(OrderDto Order) : ICommand<CreateOrderResult>;

public class CreateOrderCommandValidatior : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidatior()
    {
        RuleFor(coc => coc.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(coc => coc.Order.CustomerId).NotEmpty().WithMessage("CustomerId is required");
        RuleFor(coc => coc.Order.OrderItems).NotEmpty().WithMessage("OrderItems should not be empty");
    }
}