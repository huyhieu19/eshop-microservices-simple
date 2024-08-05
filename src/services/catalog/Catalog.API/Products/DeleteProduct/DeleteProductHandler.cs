﻿namespace Catalog.API.Products;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductCommandHandler.Handler called with {@Command}", command);
        var product = await session.LoadAsync<Product>(command.Id);
        if (product is null)
        {
            throw new ProductNotFoundException();
        }
        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync();
        return new DeleteProductResult(true);
    }
}
