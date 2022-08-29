using Downcast.UserManager.Authentication.Model;
using Downcast.UserManager.Model.Input;

namespace Downcast.UserManager.Authentication;

public interface IAuthenticationManager
{
    Task<AuthenticationResult> Authenticate(AuthenticationRequest auth);
}