using FluentValidation;
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
            // Replace the response stream with a memory stream
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // Continue processing the request
                await _next(context);
                string responseContent = "\n---Welcome to my web site!---";
                // Intercept and modify the response
                if (context.Response.StatusCode == 200)
                {
                    // Replace this with your custom logic to modify the response
                    responseContent = await FormatResponseCode200(context);
                    // Write the modified response content to the memory stream
                }
                if (context.Response.StatusCode == 401)
                {
                    // Handle exceptions here
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Internal Server Error
                    var errorMessage = "You do not have permission to access this website.";

                    // Create an ResponseModel with error details
                    var apiResponse = new CommonResponseModel<object>
                    {
                        IsSuccess = false,
                        ErrorDetail = new CommonErrorDetailModel()
                        {
                            ErrorMessage = errorMessage,
                        },
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Instance = context.Request.Path
                    };

                    // Serialize the ResponseModel to JSON
                    responseContent = JsonConvert.SerializeObject(apiResponse);
                }

                // Write the formatted response to the original response stream
                var bytes = Encoding.UTF8.GetBytes(responseContent);
                await responseBody.WriteAsync(bytes, 0, bytes.Length);

                //Copy the memory stream back to the original response stream
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        catch (Exception exception)
        {
            using (var responseBody = new MemoryStream())
            {
                (string Message, string Title, int StatusCode) details = exception switch
                {
                    InternalServerException => (
                        exception.Message,
                        exception.GetType().Name,
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError
                    ),
                    ValidationException => (
                        exception.Message,
                        exception.GetType().Name,
                        context.Response.StatusCode = StatusCodes.Status400BadRequest
                    ),
                    BadRequestException => (
                        exception.Message,
                        exception.GetType().Name,
                        context.Response.StatusCode = StatusCodes.Status400BadRequest
                    ),
                    NotFoundException => (
                        exception.Message,
                        exception.GetType().Name,
                        context.Response.StatusCode = StatusCodes.Status404NotFound
                    ),
                    _ =>
                    (
                        exception.Message,
                        exception.GetType().Name,
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError
                    )
                };


                // Handle exceptions here
                var errorMessage = details.Message;

                // Create an ResponseModel with error details
                var apiResponse = new CommonResponseModel<object>
                {
                    IsSuccess = false,
                    ErrorDetail = new CommonErrorDetailModel()
                    {
                        ErrorMessage = details.Message,
                    },
                    StatusCode = details.StatusCode,
                    Instance = context.Request.Path
                };

                if (exception is ValidationModelException validationException)
                {
                    apiResponse.ErrorDetail.ErrorCode = validationException.Data["ErrorsProperty"]!.ToString()!.ErrorConvertToCode();
                    apiResponse.ErrorDetail.ErrorType = validationException.Data["ErrorsInstanceModel"]!.ToString()!.ErrorConvertToType();
                }

                // Serialize the ResponseModel to JSON
                var formattedResponse = JsonConvert.SerializeObject(apiResponse);

                // Write the formatted response to the original response stream
                var bytes = Encoding.UTF8.GetBytes(formattedResponse);
                await originalBodyStream.WriteAsync(bytes, 0, bytes.Length);
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private async Task<string> FormatResponseCode200(HttpContext context)
    {
        // Read the original response content
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        // Parse the original response content or create your custom response format
        var responseData = JsonConvert.DeserializeObject(responseContent);

        // Create your ApiResponse<T> object
        var apiResponse = new CommonResponseModel<object>
        {
            IsSuccess = true,
            Data = responseData,
            StatusCode = context.Response.StatusCode,
            Instance = context.Request.Path
        };

        // Serialize the ApiResponse<T> object back to JSON
        var formattedResponse = JsonConvert.SerializeObject(apiResponse);

        return formattedResponse;
    }
}
