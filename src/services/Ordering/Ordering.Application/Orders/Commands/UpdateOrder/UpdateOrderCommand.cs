using FluentValidation;

namespace Ordering.Application.Orders.Commands.UpdateOrder;
public record UpdateOrderResult(bool IsSuccess);
public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(uoc => uoc.Order.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(uoc => uoc.Order.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(uoc => uoc.Order.CustomerId).NotNull().WithMessage("CustomerId is required");
    }
}
