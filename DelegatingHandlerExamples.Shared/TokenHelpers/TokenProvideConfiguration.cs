using Microsoft.Extensions.DependencyInjection;

namespace DelegatingHandlerExamples.Shared.TokenHelpers;

public static class TokenProviderConfiguration
{
    public static IServiceCollection AddTokenProvider(this IServiceCollection services, Action<Settings> action)
    {
        var settings = new Settings();
        action(settings);

        if (settings.BaseAddress is null || settings.TokenPath is null || settings.RefreshTokenPath is null) 
        {
            throw new ArgumentException("Missing settings value");
        }

        services.AddHttpClient<ITokenProvider, TokenProvider>(client =>
        {
            client.BaseAddress = settings.BaseAddress;
            return new TokenProvider(client, settings.TokenPath, settings.RefreshTokenPath);
        });

        return services;
    }

    public class Settings
    {
        public Uri? BaseAddress { get; set; }
        public string? TokenPath { get; set; }
        public string? RefreshTokenPath { get; set; }
    }
}
