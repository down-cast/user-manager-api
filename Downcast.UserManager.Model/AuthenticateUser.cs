using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.Model
{
    public class AuthenticateUser
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
