using System.Text.Json.Serialization;

namespace Downcast.UserManager.Model;

public class UserWithPassword : User
{
    [JsonIgnore]
    public PasswordInfo? PasswordInfo { get; init; }

    public bool HasPassword => PasswordInfo != null;
}