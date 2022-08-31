namespace Downcast.UserManager.Model;

public class User
{
    public string Id { get; init; } = null!;
    public string? DisplayName { get; init; }
    public string? ProfilePictureUri { get; init; }
    public SocialLinks? SocialLinks { get; init; }
    public bool? EmailValidated { get; init; }
    public string? Description { get; init; }
    public string Email { get; set; } = null!;
    public IEnumerable<string> Roles { get; init; } = Enumerable.Empty<string>();
    public DateTime Created { get; init; }
    public DateTime Updated { get; init; }
    public bool HasPicture => ProfilePictureUri != null;
    public bool HasRole(string role) => Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
}