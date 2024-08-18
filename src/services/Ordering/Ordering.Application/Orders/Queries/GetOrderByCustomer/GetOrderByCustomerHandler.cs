namespace Ordering.Application.Orders;

public class GetOrderByCustomerHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        // get orders by customer using dbContext
        // return result

        var orders = await dbContext.Orders
                        .Include(o => o.OrderItems)
                        .AsNoTracking()
                        .Where(o => o.CustomerId == CustomerId.Of(query.CustomerId))
                        .OrderBy(o => o.OrderName)
                        .ToListAsync(cancellationToken);

        return new GetOrdersByCustomerResult(orders.ConvertToOrderDtos());
    }
}
