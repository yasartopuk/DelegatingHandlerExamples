using DelegatingHandlerExamples.Shared.Contexts;
using DelegatingHandlerExamples.Shared.Middlewares;

namespace DelegatingHandlerExamples.Shared.Handlers;

public class TraceIdHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!request.Headers.Contains(TraceIdMiddlewareConfiguration.TraceIdHeaderKey))
        {
            request.Headers.Add(TraceIdMiddlewareConfiguration.TraceIdHeaderKey, TraceContext.Current.TraceId);
        }
        return base.SendAsync(request, cancellationToken);
    }
}
