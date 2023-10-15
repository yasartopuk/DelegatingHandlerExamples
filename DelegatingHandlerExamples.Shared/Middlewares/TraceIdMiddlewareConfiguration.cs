using Microsoft.AspNetCore.Builder;

namespace DelegatingHandlerExamples.Shared.Middlewares;

public static class TraceIdMiddlewareConfiguration
{
    public const string TraceIdHeaderKey = "X-Trace-Id";

    public static IApplicationBuilder UseTraceId(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TraceIdMiddleware>();
    }
}
