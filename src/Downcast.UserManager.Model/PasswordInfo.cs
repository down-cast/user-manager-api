namespace Downcast.UserManager.Model;

public class PasswordInfo
{
    public int Iterations { get; init; }
    public string Salt { get; init; } = null!;
    public string Hash { get; init; } = null!;
}