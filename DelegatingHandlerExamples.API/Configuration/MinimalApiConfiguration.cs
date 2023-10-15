using DelegatingHandlerExamples.API.DataAccess;
using DelegatingHandlerExamples.Shared.Models;
using DelegatingHandlerExamples.Shared.TokenHelpers;

namespace DelegatingHandlerExamples.API.Configuration;

public static class MinimalApiConfiguration
{
    public static void MapApi(this WebApplication app)
    {
        app.MapPost("api/login", (LoginRequest request, ITokenManager tokenManager) =>
        {
            var user = Database.Users.SingleOrDefault(u => u.Username == request.Username && u.Password == request.Password);
            if (user is null)
            {
                return Results.BadRequest(new { message = "Invalid username or password" });
            }

            var tokenModel = tokenManager.GenerateToken(user.Username);

            Database.RefreshTokens[tokenModel.RefreshToken] = user.Username;
            return Results.Ok(tokenModel);
        });

        app.MapPost("api/refreshtoken", (RefreshTokenRequest request, ITokenManager tokenManager) =>
        {
            if (request.RefreshToken is null)
            {
                return Results.BadRequest(new { message = "Invalid refresh token" });
            }

            if (!Database.RefreshTokens.TryGetValue(request.RefreshToken, out string? username))
            {
                return Results.BadRequest(new { message = "Invalid refresh token" });
            }

            var user = Database.Users.SingleOrDefault(u => u.Username == username);
            if (user is null)
            {
                return Results.BadRequest(new { message = "Invalid user" });
            }

            var tokenModel = tokenManager.GenerateToken(user.Username);
            return Results.Ok(tokenModel);
        });
    }
}
