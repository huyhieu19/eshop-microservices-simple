namespace BuildingBlocks;

public enum CErrorType
{
    Unknown = 0,
    CreateProduct = 1,
    DeleteProduct = 1,
}

public static class CErrorTypeExtensions
{
    public static CErrorType ErrorConvertToType(this string input)
    {
        switch (input)
        {
            case "createproductcommand":
                return CErrorType.CreateProduct;
            case "deleteproductcommand":
                return CErrorType.DeleteProduct;
            default:
                return CErrorType.Unknown;
        }
    }
}