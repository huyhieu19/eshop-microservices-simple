namespace BuildingBlocks;

public class ResponseModel<T>
{
    public bool IsSuccess { get; set; }
    public CErrorDetailModel? ErrorDetail { get; set; }
    public string? Instance { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; }
}

public class CErrorDetailModel
{
    public string ErrorMessage { get; set; } = string.Empty;
    public CErrorCode ErrorCode { get; set; } = CErrorCode.Unknown;
    public CErrorType ErrorType { get; set; } = CErrorType.Unknown;
}