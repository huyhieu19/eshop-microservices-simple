using Basket.API.Dtos;

namespace Basket.API.Basket.CheckoutBasket;

/// <summary>
/// Represents a request to checkout a basket.
/// </summary>
/// <remarks>
/// This record is used to encapsulate the data needed to checkout a basket.
/// </remarks>
public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);
/// <summary>
/// Represents the response for a checkout basket request.
/// </summary>
/// <remarks>
/// This record is used to encapsulate the success status of a checkout basket request.
/// </remarks>
public record CheckoutBasketResponse(bool IsSuccess);

// This class represents a module for handling checkout basket endpoints in a C# application.
// It implements the ICarterModule interface, which is a part of the Carter framework for building web APIs.
public class CheckoutBasketEndpoints : ICarterModule
{
    // This method is responsible for adding routes to the application's endpoint route builder.
    // It uses the app parameter to configure the routes.
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // This line maps a POST request to the "/basket/checkout" endpoint.
        // The lambda function passed to MapPost is executed when the request is received.
        app.MapPost("/basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
        {
            // This line converts the request object into a CheckoutBasketCommand object.
            // The Adapt method is likely part of a library Mapster, which is used to map objects of different types.
            var command = request.Adapt<CheckoutBasketCommand>();

            // This line sends the command object to a sender (likely a service bus or message queue).
            // The result is an object returned by the sender.
            var result = await sender.Send(command);

            // This line converts the result object into a CheckoutBasketResponse object.
            var response = result.Adapt<CheckoutBasketResponse>();

            // This line returns an HTTP response with a 201 status code and the response object as the body.
            return Results.Ok(response);
        })
        // These lines configure the endpoint with additional details.
        .WithName("CheckoutBasket") // Sets the name of the endpoint.
        .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created) // Specifies that the endpoint returns a CheckoutBasketResponse object with a 201 status code.
        .ProducesProblem(StatusCodes.Status400BadRequest) // Specifies that the endpoint returns a problem object with a 400 status code if there's a bad request.
        .WithSummary("Checkout Basket") // Sets the summary of the endpoint.
        .WithDescription("Checkout Basket"); // Sets the description of the endpoint.
    }
}