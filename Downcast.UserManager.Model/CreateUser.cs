using System.ComponentModel.DataAnnotations;

using Downcast.UserManager.Model.Validators;

namespace Downcast.UserManager.Model
{
    public class CreateUser
    {
        [Required(ErrorMessage = "Name must not be empty")]
        [StringLength(50, ErrorMessage = "The name cannot surpass 50 characters")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Email must not be empty")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        [StringLength(50, ErrorMessage = "Email cannot surpass 50 characters")]
        public string Email { get; set; } = null!;
        [PasswordValidator]
        public string Password { get; set; } = null!;
        [StringLength(300, ErrorMessage = "The description content cannot exceed 300 characters")]
        public string? Description { get; set; }
        [PlatformHyperlinkValidator]
        public Uri? GithubLink { get; set; }
        [PlatformHyperlinkValidator]
        public Uri? LinkedInLink { get; set; }
        [PlatformHyperlinkValidator]
        public Uri? FacebookLink { get; set; }
        [PlatformHyperlinkValidator]
        public Uri? TwitterLink { get; set; }
    }
}
