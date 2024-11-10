using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace BuildingBlocks;

public class ApiResponseMiddleware
{
    private readonly RequestDelegate _next;

    public ApiResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Capture the response stream
        var originalBodyStream = context.Response.Body;

        try
        {
            // Replace the response body with a memory stream to capture the response
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // Process the request
                await _next(context);

                // Format response based on the status code
                string responseContent = await FormatResponseContent(context);
                await WriteToOriginalStream(responseContent, originalBodyStream);
            }
        }
        catch (Exception exception)
        {
            // Handle and write exception response
            await HandleExceptionAsync(context, exception, originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private async Task<string> FormatResponseContent(HttpContext context)
    {
        // Reset and read the response body
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseData = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        // Check if the response indicates an error
        bool isError = context.Response.StatusCode >= 400;
        string errorMessage = isError ? GetErrorMessage(context.Response.StatusCode) : null;

        // Create your ApiResponse<T> object
        var apiResponse = new CommonResponseModel<object>
        {
            IsSuccess = !isError,
            ErrorDetail = isError ? new CommonErrorDetailModel
            {
                ErrorMessage = errorMessage! // You can set a custom error message here
            } : null,
            Data = !isError ? JsonConvert.DeserializeObject(responseData) : null,
            StatusCode = context.Response.StatusCode,
            Instance = context.Request.Path
        };
        // Serialize the ApiResponse<T> object back to JSON
        var formattedResponse = JsonConvert.SerializeObject(apiResponse);

        return formattedResponse;
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception, Stream originalBodyStream)
    {
        // Clear the response and set the status code
        context.Response.Clear();
        context.Response.StatusCode = 500; // Internal Server Error

        var apiResponse = new CommonResponseModel<object>
        {
            IsSuccess = false,
            ErrorDetail = new CommonErrorDetailModel
            {
                ErrorMessage = exception.Message // You can set a custom error message here
            },
            Data = exception.Message,
            StatusCode = context.Response.StatusCode,
            Instance = context.Request.Path
        };

        // Serialize the error response to JSON
        var errorResponse = JsonConvert.SerializeObject(apiResponse);
        await WriteToOriginalStream(errorResponse, originalBodyStream);
    }

    private async Task WriteToOriginalStream(string content, Stream originalStream)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        await originalStream.WriteAsync(bytes, 0, bytes.Length);
        originalStream.Seek(0, SeekOrigin.Begin);
    }

    private static string GetErrorMessage(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad Request - The server could not understand the request due to invalid syntax.",
            401 => "Unauthorized - The client must authenticate itself to get the requested response.",
            403 => "Forbidden - The client does not have access rights to the content.",
            404 => "Not Found - The server cannot find the requested resource.",
            500 => "Internal Server Error - The server has encountered an unexpected error.",
            _ => "An error occurred.",
        };
    }
}
