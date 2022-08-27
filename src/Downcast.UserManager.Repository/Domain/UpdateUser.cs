namespace Downcast.UserManager.Repository.Domain;

public class UpdateUser
{
    internal string? DisplayName { get; set; }

    internal string? ProfilePictureUri { get; set; }
    
    internal SocialLinks? SocialLinks { get; set; }

}