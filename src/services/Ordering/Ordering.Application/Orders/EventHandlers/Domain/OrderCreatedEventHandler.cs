using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers;

public class OrderCreatedEventHandler(
    IPublishEndpoint publishEndpoint,
    ILogger<OrderCreatedEventHandler> logger,
    IFeatureManager featureManager
    )
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
        if (await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            var orderCreateIntergrationEvent = domainEvent.order.ToOrderDto();
            await publishEndpoint.Publish(orderCreateIntergrationEvent, cancellationToken);
        }
    }
}
