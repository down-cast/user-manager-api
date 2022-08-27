namespace Downcast.UserManager.Model;

public class User : UpdateUser
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public PasswordInfo? PasswordInfo { get; set; }
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public bool HasPassword => PasswordInfo != null;
    public bool HasPicture => ProfilePictureUri != null;
    public bool HasRole(string role) => Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
}