namespace Catalog.API.Products;

// Cannot change name
public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidatior : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidatior()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}

internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductCommandHandler.Handler called with {@Command}", command);
        var product = await session.LoadAsync<Product>(command.Id);
        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync();
        return new DeleteProductResult(true);
    }
}
