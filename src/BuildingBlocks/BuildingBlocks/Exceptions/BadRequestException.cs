namespace BuildingBlocks;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, string details) : base(message)
    {
        Details = details;
    }


    public string? Details { get; }
}

public class ValidationModelException : Exception
{
    public object ErrorsInstanceModel { get; }
    public string ErrorsProperty { get; }

    public ValidationModelException(string message, object _ErrorsInstanceModel, string _ErrorsProperty) : base(message)
    {
        ErrorsInstanceModel = _ErrorsInstanceModel;
        ErrorsProperty = _ErrorsProperty;
        Data["ErrorsInstanceModel"] = _ErrorsInstanceModel;
        Data["ErrorsProperty"] = _ErrorsProperty;
    }

}