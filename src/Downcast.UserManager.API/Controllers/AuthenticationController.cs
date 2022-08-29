using Downcast.UserManager.Authentication;
using Downcast.UserManager.Authentication.Model;
using Downcast.UserManager.Model.Input;

using Microsoft.AspNetCore.Mvc;

namespace Downcast.UserManager.API.Controllers;

[ApiController]
[Route("api/v1/users/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationManager _authenticationManager;

    public AuthenticationController(IAuthenticationManager authenticationManager)
    {
        _authenticationManager = authenticationManager;
    }


    /// <summary>
    /// Authenticates a user given its email and password
    /// </summary>
    /// <param name="authRequest"></param>
    /// <returns>Jwt token with its expiration date</returns>
    [HttpPost]
    public Task<AuthenticationResult> Authenticate([FromBody] AuthenticationRequest authRequest)
    {
        return _authenticationManager.Authenticate(authRequest);
    }
}