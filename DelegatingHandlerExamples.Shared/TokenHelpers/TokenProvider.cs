using DelegatingHandlerExamples.Shared.Models;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;

namespace DelegatingHandlerExamples.Shared.TokenHelpers;

public class TokenProvider : ITokenProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _tokenPath;
    private readonly string _refreshTokenPath;

    public TokenProvider(HttpClient httpClient,
        string tokenPath,
        string refreshTokenPath)
    {
        _httpClient = httpClient;
        _tokenPath = tokenPath;
        _refreshTokenPath = refreshTokenPath;
    }

    public async Task<TokenModel> SignInAsync(string username, string password)
    {
        var payload = JsonSerializer.Serialize(new LoginRequest
        {
            Username = username,
            Password = password
        });

        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_tokenPath, content);
        
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadFromJsonAsync<TokenModel>();
            return token ?? throw new UnauthorizedAccessException();
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new UnauthorizedAccessException(message);
        }
    }

    public async Task<TokenModel> RefreshTokenAsync(string refreshToken)
    {
        var payload = JsonSerializer.Serialize(new RefreshTokenRequest
        {
            RefreshToken = refreshToken,
        });

        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_refreshTokenPath, content);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadFromJsonAsync<TokenModel>();
            return token ?? throw new UnauthorizedAccessException();
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new UnauthorizedAccessException(message);
        }
    }
}