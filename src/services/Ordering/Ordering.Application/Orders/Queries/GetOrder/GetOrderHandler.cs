namespace Ordering.Application.Orders;

public class GetOrderHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        // get orders with pagination
        // return result

        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

        var orders = await dbContext.Orders
                       .Include(o => o.OrderItems)
                       .OrderBy(o => o.OrderName)
                       .Paginate(pageNumber: pageIndex, pageSize)
                       .ToListAsync(cancellationToken);
        return new GetOrdersResult(
        new PaginatedResult<OrderDto>(
            pageIndex,
            pageSize,
            totalCount,
            orders.ConvertToOrderDtos()));
    }
}
