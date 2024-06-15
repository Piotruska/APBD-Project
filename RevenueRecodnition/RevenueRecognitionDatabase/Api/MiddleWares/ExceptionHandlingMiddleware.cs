using Microsoft.AspNetCore.Diagnostics;
using RevenueRecodnition.Api.Exeptions;

namespace RevenueRecodnition.Api.MiddleWares;

public class ExceptionHandlingMiddleware : IExceptionHandler
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        this._logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {

        if (exception is NotFoundExeption)  
        {
            var exceptionMessage = exception.Message;
            _logger.LogError(
                "NotFound exception: {exceptionMessage}, Time of occurrence {time}",
                exceptionMessage, DateTime.UtcNow);
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Message=$"NotFound exception: {exceptionMessage}, Time of occurrence {DateTime.UtcNow}"
            }, cancellationToken: cancellationToken);
            return true;
        }
        
        if (exception is BadRequestExeption)  
        {
            var exceptionMessage = exception.Message;
            _logger.LogError(
                "BadRequest exception: {exceptionMessage}, Time of occurrence {time}",
                exceptionMessage, DateTime.UtcNow);
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Message=$"BadRequest exception: {exceptionMessage}, Time of occurrence {DateTime.UtcNow}"
            }, cancellationToken: cancellationToken);
            return true;
        }
        
        if (exception is UnauthorizedExeption)  
        {
            var exceptionMessage = exception.Message;
            _logger.LogError(
                "Unauthorized exception: {exceptionMessage}, Time of occurrence {time}",
                exceptionMessage, DateTime.UtcNow);
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Message=$"Unauthorized exception: {exceptionMessage}, Time of occurrence {DateTime.UtcNow}"
            }, cancellationToken: cancellationToken);
            return true;
        }

        return false;
    }
}