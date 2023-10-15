using DelegatingHandlerExamples.Shared.Models;

namespace DelegatingHandlerExamples.API.DataAccess;

public static class Database
{
    public static IDictionary<string, string> RefreshTokens { get; set; } = new Dictionary<string, string>();

    public static List<User> Users = new()
    {
        new User { Username = "admin", Password = "123", Role = "Admin" },
        new User { Username = "yasar", Password = "123", Role = "User" }
    };
}
