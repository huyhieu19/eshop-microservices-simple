namespace Ordering.Application.Orders.Queries;

public record GetOrdersByCustomerResult(IEnumerable<OrderDto> orderDtos);
public record GetOrderByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerResult>;