using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks;
public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures =
            validationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count != 0)
        {
            string message = failures.Select(p => p.ErrorMessage).FirstOrDefault()!;
            string propertiesName = failures.Select(p => p.PropertyName).FirstOrDefault()!;
            //throw new ValidationModelException(message, nameof(TRequest).ConvertToType(), propertiesName.ConvertListToCode());
            throw new ValidationModelException(message, context.InstanceToValidate.GetType().Name, propertiesName);
        }

        return await next();
    }
}
