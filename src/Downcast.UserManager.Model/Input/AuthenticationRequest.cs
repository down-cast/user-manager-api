using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Model.Input;

public class AuthenticationRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;

    [Required]
    public string Password { get; init; } = null!;
}