namespace Downcast.UserManager.Authentication.Model;

public class AuthenticationResult
{
    public string Token { get; set; } = null!;
    public DateTime ExpirationDate { get; set; }
}