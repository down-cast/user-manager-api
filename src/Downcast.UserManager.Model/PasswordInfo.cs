namespace Downcast.UserManager.Model;

public class PasswordInfo
{
    public required int Iterations { get; set; }
    public required string Salt { get; set; }
    public required string Hash { get; set; }
}