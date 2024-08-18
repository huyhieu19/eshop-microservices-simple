namespace Ordering.Application.Orders;

public record GetOrdersByNameQuery(string Name) : IQuery<GetOrdersByNameResult>;
public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);