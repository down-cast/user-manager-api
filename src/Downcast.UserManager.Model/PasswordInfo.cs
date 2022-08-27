namespace Downcast.UserManager.Model;

public class PasswordInfo
{
    public int Iterations { get; set; }
    public string Salt { get; set; } = null!;
    public string Hash { get; set; } = null!;
}