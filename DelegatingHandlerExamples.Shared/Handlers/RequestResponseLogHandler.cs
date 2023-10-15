using DelegatingHandlerExamples.Shared.Contexts;
using DelegatingHandlerExamples.Shared.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DelegatingHandlerExamples.Shared.Handlers;

public class RequestResponseLogHandler : DelegatingHandler
{
    private readonly ILogger<RequestResponseLogHandler> _logger;

    public RequestResponseLogHandler(ILogger<RequestResponseLogHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var logModel = new RequestResponseLogModel
        {
            TraceId = TraceContext.Current.TraceId,
            RequestUri = request.RequestUri?.AbsoluteUri,
            Method = request.Method.ToString()
        };

        if (request.Content != null)
        {
            logModel.RequestBody = await request.Content.ReadAsStringAsync(cancellationToken);
        }

        var stopwatch = Stopwatch.StartNew();
        var response = await base.SendAsync(request, cancellationToken);
        stopwatch.Stop();

        logModel.TimeElapsed = stopwatch.Elapsed;
        logModel.StatusCode = (int)response.StatusCode;
        logModel.ResponseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        _logger.LogInformation($"RequestResponseLog:\n{logModel}");
        return response;
    }
}
