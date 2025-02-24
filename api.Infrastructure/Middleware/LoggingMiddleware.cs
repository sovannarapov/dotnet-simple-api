using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace api.Infrastructure.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("Handling request: {Method} {Url}", context.Request.Method, context.Request.Path);

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation(
            "Finished handling request. Status code: {StatusCode}. Time taken: {ElapsedMilliseconds} ms", 
            context.Response.StatusCode, 
            stopwatch.ElapsedMilliseconds
        );
    }
}
