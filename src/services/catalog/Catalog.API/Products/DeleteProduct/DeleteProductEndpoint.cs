namespace Catalog.API.Products;

public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{Id}", async (Guid Id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(Id));
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(result);
        })
        .WithName("DeleteProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Product")
        .WithDescription("Delete Product");
    }
}
