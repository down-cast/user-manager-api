using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Model;

public class UpdateUserInputModel
{
    [MaxLength(200)]
    public string? DisplayName { get; init; }

    public string? ProfilePictureUri { get; init; }
    public SocialLinks? SocialLinks { get; init; }
    public bool? EmailValidated { get; init; }

    [MaxLength(1500)]
    public string? Description { get; init; }
}