namespace Downcast.UserManager.Model;

public class CreateUser
{
    public string Email { get; init; } = null!;
    public string? DisplayName { get; init; }
    public PasswordInfo? PasswordInfo { get; init; }

    public IEnumerable<string> Roles { get; init; } = Enumerable.Empty<string>();
}