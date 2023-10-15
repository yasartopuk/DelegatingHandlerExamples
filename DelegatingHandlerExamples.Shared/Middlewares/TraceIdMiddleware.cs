using DelegatingHandlerExamples.Shared.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace DelegatingHandlerExamples.Shared.Middlewares;

public class TraceIdMiddleware
{
    private readonly RequestDelegate _next;

    public TraceIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(TraceIdMiddlewareConfiguration.TraceIdHeaderKey, out StringValues traceId))
        {
            traceId = Guid.NewGuid().ToString().ToUpper();
        }

        context.TraceIdentifier = traceId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Add(TraceIdMiddlewareConfiguration.TraceIdHeaderKey, context.TraceIdentifier);
            return Task.CompletedTask;
        });

        using (var trace = TraceContext.Begin(traceId))
        {
            await _next.Invoke(context);
        }
    }
}


