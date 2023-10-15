using DelegatingHandlerExamples.Shared.Models;
using System.Security.Claims;

namespace DelegatingHandlerExamples.Shared.TokenHelpers
{
    public interface ITokenManager
    {
        string GenerateRefreshToken();
        TokenModel GenerateToken(string userName);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}