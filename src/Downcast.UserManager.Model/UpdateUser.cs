namespace Downcast.UserManager.Model;

public class UpdateUser
{
    public string? DisplayName { get; set; }
    public string? ProfilePictureUri { get; set; }
    public SocialLinks? SocialLinks { get; set; }
}