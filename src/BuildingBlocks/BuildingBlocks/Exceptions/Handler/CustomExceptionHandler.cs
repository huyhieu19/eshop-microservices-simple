//using FluentValidation;
//using Microsoft.AspNetCore.Diagnostics;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace BuildingBlocks;

//public class CustomExceptionHandler : IExceptionHandler
//{
//    private readonly ILogger<CustomExceptionHandler> logger;
//    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
//    {
//        this.logger = logger;
//    }
//    public async ValueTask<bool> TryHandleAsync(
//        HttpContext httpContext,
//        Exception exception,
//        CancellationToken cancellationToken)
//    {
//        var exceptionMessage = exception.Message;
//        logger.LogError(
//            "Error Message: {exceptionMessage}, Time of occurrence {time}",
//            exceptionMessage, DateTime.UtcNow);
//        // Return false to continue with the default behavior
//        // - or - return true to signal that this exception is handled
//        (string Detail, string Title, int StatusCode) details = exception switch
//        {
//            InternalServerException => (
//                exception.Message,
//                exception.GetType().Name,
//                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
//            ),
//            ValidationException => (
//                exception.Message,
//                exception.GetType().Name,
//                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
//            ),
//            BadRequestException => (
//                exception.Message,
//                exception.GetType().Name,
//                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
//            ),
//            NotFoundException => (
//                exception.Message,
//                exception.GetType().Name,
//                httpContext.Response.StatusCode = StatusCodes.Status404NotFound
//            ),
//        };

//        var problemDetails = new ProblemDetails
//        {
//            Title = details.Title,
//            Detail = details.Detail,
//            Status = details.StatusCode,
//            Instance = httpContext.Request.Path
//        };

//        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

//        if (exception is ValidationException validationException)
//        {
//            problemDetails.Extensions.Add("ValidationError", validationException.Message);
//        }

//        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

//        return true;
//    }
//}
