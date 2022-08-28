namespace Downcast.UserManager.Model;

public class User : UpdateUser
{
    public required string Id { get; init; }
    public required string Email { get; set; }
    public PasswordInfo? PasswordInfo { get; set; }
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public bool HasPassword => PasswordInfo != null;
    public bool HasPicture => ProfilePictureUri != null;
    public bool HasRole(string role) => Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
}