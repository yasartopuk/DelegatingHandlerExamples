using DelegatingHandlerExamples.Shared.TokenHelpers;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Headers;

namespace DelegatingHandlerExamples.Shared.Handlers;

public class RefreshTokenHandler : DelegatingHandler
{
    private readonly ITokenProvider _tokenProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefreshTokenHandler(ITokenProvider tokenProvider, IHttpContextAccessor httpContextAccessor)
    {
        _tokenProvider = tokenProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = GetUserClaimsValue("AccessToken");
        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var refreshToken = GetUserClaimsValue("RefreshToken");
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var token = await _tokenProvider.RefreshTokenAsync(refreshToken);
                if (!string.IsNullOrEmpty(token.AccessToken))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }
        }

        return response;
    }

    private string? GetUserClaimsValue(string claimsType)
    {
        return _httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true
             ? _httpContextAccessor.HttpContext.User.FindFirst(claimsType)?.Value
             : null;
    }
}

