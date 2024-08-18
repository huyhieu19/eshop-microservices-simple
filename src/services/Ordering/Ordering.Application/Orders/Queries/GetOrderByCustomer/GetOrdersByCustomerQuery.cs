namespace Ordering.Application.Orders;
public record GetOrdersByCustomerResult(IEnumerable<OrderDto> orderDtos);
public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerResult>;