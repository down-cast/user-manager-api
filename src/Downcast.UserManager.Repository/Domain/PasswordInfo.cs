namespace Downcast.UserManager.Repository.Domain;

internal class PasswordInfo
{
    internal int Iterations { get; set; }
    internal string Salt { get; set; } = null!;
    internal string Hash { get; set; } = null!;
}