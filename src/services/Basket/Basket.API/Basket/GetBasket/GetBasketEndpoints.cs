namespace Basket.API.Basket;

// public record GetBasketRequest(string UserName);

public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoints : ICarterModule
{
    /// <summary>
    /// Adds routes to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="app">The <see cref="IEndpointRouteBuilder"/> to add routes to.</param>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Maps a GET request to "/basket/{userName}" and handles it asynchronously.
        app.MapGet("basket/{userName}", async (string userName, ISender sender) =>
        {
            // Sends a GetBasketQuery with the specified userName to the sender and waits for the result.
            var result = await sender.Send(new GetBasketQuery(userName));

            // Adapts the result to a GetBasketResponse and assigns it to the response variable.
            var response = result.Adapt<GetBasketResponse>();

            // Returns the response as a 200 OK result.
            return Results.Ok(response);
        })
        // Assigns the name "GetBasket" to the route.
        .WithName("GetBasket")
        // Specifies that the route produces a GetBasketResponse with a 200 OK status code.
        .Produces<GetBasketResponse>(StatusCodes.Status200OK)
        // Specifies that the route produces a problem with a 400 Bad Request status code.
        .ProducesProblem(StatusCodes.Status400BadRequest)
        // Adds a summary of the route as "Get Basket".
        .WithSummary("Get Basket")
        // Adds a description of the route as "Get Basket".
        .WithDescription("Get Basket");
    }
}
