using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Model.Input;

public class AuthenticationRequest : UpdatePasswordInput
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;
}