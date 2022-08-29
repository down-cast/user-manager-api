using Downcast.UserManager.Model.Input;

namespace Downcast.UserManager.Authentication;

public interface IAuthenticationManager
{
    Task<bool> ValidateCredentials(AuthenticationRequest auth);
}