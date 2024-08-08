namespace BuildingBlocks;

public enum CErrorType
{
    Unknown = 0,
    Product = 1,
    Category = 2,
}

public static class CErrorTypeExtensions
{
    public static CErrorType ErrorConvertToType(this string input)
    {
        return input.ToLower() switch
        {
            "createproductcommand" or "deleteproductcommand" => CErrorType.Product,

            _ => CErrorType.Unknown,
        };
    }
}