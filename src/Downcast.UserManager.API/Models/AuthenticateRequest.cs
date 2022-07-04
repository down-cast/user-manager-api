using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.API.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
