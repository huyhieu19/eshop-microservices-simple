namespace Catalog.API.Products;

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid Id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(Id));

            var response = result.Adapt<GetProductByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By I");
    }
}
