using System.Text.Json.Serialization;

namespace Downcast.UserManager.Model;

public class CreateUser : UpdateUser
{
    public string Email { get; set; } = null!;

    [JsonIgnore]
    public PasswordInfo? PasswordInfo { get; set; }

    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
}