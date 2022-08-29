using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Model.Input;

public class CreateUserInputModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    public string? DisplayName { get; init; }
}