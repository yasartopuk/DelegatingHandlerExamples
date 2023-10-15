using Polly;
using Polly.Retry;

namespace DelegatingHandlerExamples.Shared.Handlers;

public class RetryHandler : DelegatingHandler
{
    private const int RetryCount = 3;
    private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy = Policy
        .HandleResult<HttpResponseMessage>(CheckHttpStatusCode)
        .Or<Exception>()
        .WaitAndRetryAsync(RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await RetryPolicy.ExecuteAsync(async () =>
        {
            return await base.SendAsync(request, cancellationToken);
        });

        return response;
    }

    private static bool CheckHttpStatusCode(HttpResponseMessage response)
    {
        return (int)response.StatusCode >= 500 && (int)response.StatusCode <= 599;
    }
}
