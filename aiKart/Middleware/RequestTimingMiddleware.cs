using System.Diagnostics;
namespace aiKart.Middleware;
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        await _next(context);
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;

        _logger.LogInformation($"Request [{context.Request.Method}] {context.Request.Path} took {elapsedMs} ms");
    }
}
