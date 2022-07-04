using System.ComponentModel.DataAnnotations;

namespace Downcast.UserManager.API.Models
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [StringLength(300, ErrorMessage = "The description content cannot exceed 300 characters. ")]
        public string Description { get; set; }
        public string GithubLink { get; set; }
        public string LinkedInLink { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
    }
}
