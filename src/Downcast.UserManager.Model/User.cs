namespace Downcast.UserManager.Model;

public class User
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePictureUri { get; set; }
    public PasswordInfo? PasswordInfo { get; set; }
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();

    public bool HasPassword => PasswordInfo != null;
    public bool HasPicture => ProfilePictureUri != null;
    public string FullName => $"{FirstName} {LastName}";
    public bool HasRole(string role) => Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
}