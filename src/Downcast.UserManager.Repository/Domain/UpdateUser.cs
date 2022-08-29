namespace Downcast.UserManager.Repository.Domain;

public class UpdateUser
{
    internal string? DisplayName { get; init; }

    internal string? ProfilePictureUri { get; init; }
    internal string? Description { get; init; }

    internal SocialLinks? SocialLinks { get; init; }
    public bool? EmailValidated { get; init; }
}