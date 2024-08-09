namespace BuildingBlocks;

public enum CErrorCode
{
    Unknown = 0,
    // Product
    ProductId = 100,
    ProductName = 101,
    ProductCategory = 102,
    ProductImageFile = 103,
    ProductPrice = 104,

    //

}
public static class CErrorCodeExtensions
{
    public static CErrorCode ErrorConvertToCode(this string property)
    {
        return property.ToLower() switch
        {
            "name" => CErrorCode.ProductName,
            "category" => CErrorCode.ProductCategory,
            "imagefile" => CErrorCode.ProductImageFile,
            "price" => CErrorCode.ProductPrice,
            _ => CErrorCode.Unknown,
        };
    }
}