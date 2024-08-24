namespace Ordering.API.Endpoints;

/// <summary>
/// Accepts a UpdateOrderRequest.
/// Maps the request to an UpdateOrderCommand.
/// Sends the command for processing.
/// Returns a success or error response based on the outcome.
/// </summary>

public record UpdateOrderRequest(OrderDto orderDto);
public record UpdateOrderResponse(bool IsSuccess);

public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
        {
            var orderCommand = request.Adapt<UpdateOrderCommand>();
            var result = await sender.Send(orderCommand);
            var response = result.Adapt<UpdateOrderResponse>();
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateOrder))
        .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Order")
        .WithDescription("Update Order");
    }
}
