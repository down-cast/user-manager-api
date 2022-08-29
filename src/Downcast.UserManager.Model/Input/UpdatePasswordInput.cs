using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Model.Input;

public class UpdatePasswordInput
{
    [Required(AllowEmptyStrings = false)]
    public string Password { get; init; } = null!;
}