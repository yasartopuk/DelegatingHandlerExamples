namespace DelegatingHandlerExamples.Shared.Models;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; } // Gerçekte şifreleri hash'lenmiş olarak saklamalısınız!
    public string Role { get; set; }
}
