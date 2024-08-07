namespace BuildingBlocks;

public enum CErrorCode
{
    Unknown = 0,
    // Product
    ProductName = 1,
    ProductCategory = 2,
    ProductImageFile = 3,
    ProductPrice = 4,

    //
}
public static class CErrorCodeExtensions
{
    public static CErrorCode ErrorConvertToCode(this string property)
    {
        switch (property.ToLower())
        {
            case "name":
                return CErrorCode.ProductName;
            case "category":
                return CErrorCode.ProductCategory;
            case "imagefile":
                return CErrorCode.ProductImageFile;
            case "price":
                return CErrorCode.ProductPrice;
            default:
                return CErrorCode.Unknown;
        }
    }
}