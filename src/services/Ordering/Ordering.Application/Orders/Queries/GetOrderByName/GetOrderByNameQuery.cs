namespace Ordering.Application.Orders.Queries;

public record GetOrderByNameQuery(string Name) : IQuery<GetOrderByNameResult>;
public record GetOrderByNameResult(IEnumerable<OrderDto> Orders);