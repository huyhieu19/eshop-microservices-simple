namespace Ordering.Domain.Models;

public class OrderItem : Entity<OrderItemId>
{
    public OrderItemId OrderId { get; private set; } = default!;
    public ProductId ProductId { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;

    internal OrderItem(OrderItemId orderId, ProductId productId, int quantity, decimal price)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
}
