namespace Ordering.Application.Orders.EventHandlers;

public class OrderUpdateEventHandler(ILogger<OrderUpdateEventHandler> logger) : INotificationHandler<OrderUpdatedEvent>
{
    public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask; 
    }
}
