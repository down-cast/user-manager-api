namespace Downcast.UserManager.Repository.Domain;

internal class User
{
    public string Id { get; set; } = null!;
    internal string Email { get; set; } = null!;
    internal string? FirstName { get; set; }
    internal string? LastName { get; set; }
    internal string? ProfilePictureUri { get; set; }
    internal PasswordInfo PasswordInfo { get; set; } = null!;
    internal IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}