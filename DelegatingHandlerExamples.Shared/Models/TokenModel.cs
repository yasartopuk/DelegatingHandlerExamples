using System.Text.Json.Serialization;

namespace DelegatingHandlerExamples.Shared.Models;

public class TokenModel
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "Bearer";

    [JsonPropertyName("expires_in")]
    public int ExpiresInMinutes { get; set; }

    public TokenModel(string accessToken, string refreshToken, int expiresInMinutes)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        ExpiresInMinutes = expiresInMinutes;
    }
}
