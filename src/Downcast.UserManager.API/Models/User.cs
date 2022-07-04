using System.Text.Json.Serialization;

namespace Downcast.UserManager.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string GithubLink { get; set; }
        public string LinkedInLink { get; set; }
        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string PasswordHash { get; set; }
    }
}
