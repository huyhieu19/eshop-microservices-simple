﻿namespace Basket.API.Basket;

public record StoreBasketRequest(ShoppingCart Cart);

public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", StoreBasket)
            .WithName("StoreBasket")
            .Produces<StoreBasketResponse>(statusCode: 201)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Store Basket")
            .WithDescription("Store Basket");
    }

    private async Task<IResult> StoreBasket(StoreBasketRequest request, ISender sender)
    {
        var command = request.Adapt<StoreBasketCommand>();
        var result = await sender.Send(command);
        var response = result.Adapt<StoreBasketResponse>();
        return Results.Created($"/basket/{response.UserName}", response);
    }
}
