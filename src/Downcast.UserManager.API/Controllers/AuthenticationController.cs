using Downcast.UserManager.Authentication;
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
    /// Validates credentials of a user given its email and password
    /// </summary>
    /// <param name="authRequest"></param>
    /// <returns>Jwt token with its expiration date</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ValidateCredentials([FromBody] AuthenticationRequest authRequest)
    {
        bool credentialsValid = await _authenticationManager.ValidateCredentials(authRequest).ConfigureAwait(false);
        if (credentialsValid)
        {
            return Ok();
        }

        return Unauthorized();
    }
}