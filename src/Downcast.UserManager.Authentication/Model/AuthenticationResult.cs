namespace Downcast.UserManager.Authentication.Model;

public class AuthenticationResult
{
    public string Token { get; init; } = null!;
    public DateTime ExpirationDate { get; init; }
}