using BuildingBlocks;

namespace Basket.API.Exceptions;

public class BasketNotFoundException : NotFoundException
{
    public BasketNotFoundException(string name) : base("Basket", name)
    {
    }
}
