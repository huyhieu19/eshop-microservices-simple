namespace Ordering.API.Endpoints;

//- Accepts the order ID as a parameter.
//- Constructs a DeleteOrderCommand.
//- Sends the command using MediatR.
//- Returns a success or not found response.

//public record DeleteOrderRequest(Guid orderId);
public record DeleteOrderResponse(bool IsSuccess);

public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteOrderCommand(id));
            var response = result.Adapt<DeleteOrderResponse>();
            return Results.Ok(response);
        })
        .WithName(nameof(DeleteOrder))
        .Produces<DeleteOrderResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Order")
        .WithDescription("Delete Order");
    }
}
