namespace Downcast.UserManager.Model;

public class User : CreateUserInputModel
{
    public string Id { get; init; } = null!;
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public bool HasPassword => PasswordInfo != null;
    public bool HasPicture => ProfilePictureUri != null;
    public bool HasRole(string role) => Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
}