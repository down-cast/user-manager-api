namespace Downcast.UserManager.Authentication.Model;

public class AuthenticationResult
{
    public required string Token { get; init; }
    public required DateTime ExpirationDate { get; init; }
}