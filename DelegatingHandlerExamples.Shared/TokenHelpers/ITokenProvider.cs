using DelegatingHandlerExamples.Shared.Models;

namespace DelegatingHandlerExamples.Shared.TokenHelpers;

public interface ITokenProvider
{
    Task<TokenModel> SignInAsync(string username, string password);
    Task<TokenModel> RefreshTokenAsync(string refreshToken);
}
